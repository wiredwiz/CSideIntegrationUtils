using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   public class ClientRepository
   {
      private static ClientRepository _Default;
      private readonly Dictionary<long, Client> _RunningClients;
      private readonly SynchronizationContext _Context;

      /// <summary>
      /// Initializes a new instance of the <see cref="ClientRepository"/> class.
      /// </summary>
      public ClientRepository()
      {
         _Context = SynchronizationContext.Current;
         _RunningClients = new Dictionary<long, Client>();
         GetClients();
      }

      public static ClientRepository Default => _Default ?? (_Default = new ClientRepository());


      public delegate void NewClientDetectedEventHandler(object sender, Client client);

      /// <summary>
      /// Occurs when the repository detects a new client instance.
      /// </summary>
      public event NewClientDetectedEventHandler NewClientDetected;

      [DllImport("user32.dll", SetLastError = true)]
      private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

      /// <summary>
      /// Returns a pointer to the IRunningObjectTable interface on the local running object table (ROT).
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="prot">The address of an IRunningObjectTable* pointer variable that receives the interface pointer to the local ROT.
      /// When the function is successful, the caller is responsible for calling Release on the interface pointer. If an error occurs, *pprot is undefined.</param>
      [DllImport("ole32.dll")]
      private static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

      /// <summary>
      /// Returns a pointer to an implementation of IBindCtx (a bind context object).
      /// This object stores information about a particular moniker-binding operation.
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="ppbc">Address of an IBindCtx* pointer variable that receives
      /// the interface pointer to the new bind context object. When the function is
      /// successful, the caller is responsible for calling Release on the bind context.
      /// A NULL value for the bind context indicates that an error occurred.</param>
      /// <returns>This function can return the standard return values E_OUTOFMEMORY and S_OK.</returns>
      [DllImport("ole32.dll")]
      private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

      /// <summary>
      /// Posts the new client event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostNewClientDetectedEvent(object state)
      {
         NewClientDetected?.Invoke(this, state as Client);
      }

      private static long PackKey(int windowHandle, uint processId)
      {
         long key = windowHandle;
         key <<= 32;
         key |= processId;
         return key;
      }

      private static void UnpackKey(long key, out int windowHandle, out int processId)
      {
         windowHandle = (int)(key >> 32);
         processId = (int)key;
      }

      /// <summary>
      /// Gets the active client list.
      /// </summary>
      /// <returns>A list of designer objects corresponding to running client instances</returns>
      /// <remarks>If there are multiple instances with the same database and company, only the first is exposed</remarks>
      protected virtual List<object> GetActiveClientList()
      {
         var clientList = new List<object>();

         var numFetched = IntPtr.Zero;
         IRunningObjectTable runningObjectTable = null;
         IEnumMoniker monikerEnumerator = null;
         IBindCtx ctx = null;
         var monikers = new IMoniker[1];

         try
         {
            GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();
            while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
            {
               CreateBindCtx(0, out ctx);

               monikers[0].GetDisplayName(ctx, null, out var runningObjectName);

               runningObjectTable.GetObject(monikers[0], out var runningObjectVal);

               if (!string.IsNullOrEmpty(runningObjectName) && (runningObjectName.IndexOf("!C/SIDE!navision://client/run?", StringComparison.Ordinal) != -1) &&
                   (runningObjectName.IndexOf("database=", StringComparison.Ordinal) != -1) &&
                      !clientList.Contains(runningObjectVal))
                  {
                     clientList.Add(runningObjectVal);
                  }
                  else if (runningObjectVal != null)
                     Marshal.ReleaseComObject(runningObjectVal);

               if (ctx != null)
                     Marshal.ReleaseComObject(ctx);
            }
         }
         finally
         {
            // Free resources
            if (runningObjectTable != null)
               Marshal.ReleaseComObject(runningObjectTable);
            if (monikerEnumerator != null)
               Marshal.ReleaseComObject(monikerEnumerator);
            if (ctx != null)
               Marshal.ReleaseComObject(ctx);
         }

         return clientList;
      }

      protected virtual void UpdateClientCache(List<object> clientList)
      {
         // First remove all dead clients from our list by checking whether their process is alive
         var pids = GetRunningProcessIds();
         foreach (var pair in _RunningClients.ToList())
         {
            if (!pids.Contains(pair.Value.ProcessId))
               _RunningClients.Remove(pair.Key);
         }

         // Now we loop through our new list of clients and reconcile it against our previously known clients
         foreach (IObjectDesigner designer in clientList)
         {
            int handle;
            try
            {
               INSHyperlink applicationInstance = designer as INSHyperlink;
               // If we can't retrieve an INSHyperlink reference then the likely hood is that the client is no longer valid.
               if (applicationInstance == null)
                  continue;
               applicationInstance.GetNavWindowHandle(out handle);
            }
            catch (Exception)
            {
               continue;
            }

            GetWindowThreadProcessId((IntPtr)handle, out var pid);
            var key = PackKey(handle, pid);
            if (!_RunningClients.ContainsKey(key))
               _RunningClients[key] = new Client(designer, key, handle, (int)pid);
         }
      }

      protected virtual List<int> GetRunningProcessIds()
      {
         var clientProcesses = new List<int>();
         foreach (Process process in Process.GetProcesses())
         {
            if (string.Compare(process.ProcessName, "finsql", StringComparison.OrdinalIgnoreCase) == 0 ||
                string.Compare(process.ProcessName, "fin", StringComparison.OrdinalIgnoreCase) == 0)
               clientProcesses.Add(process.Id);
         }

         return clientProcesses;
      }

      internal void RaiseNewClientDetected(Client client)
      {
         if (NewClientDetected != null)
         {
            if (_Context != null)
               _Context.Post(PostNewClientDetectedEvent, client);
            else
               PostNewClientDetectedEvent(client);
         }
      }

      public virtual List<Client> GetClients()
      {
         var designers = GetActiveClientList();
         UpdateClientCache(designers);
         return _RunningClients?.Values.ToList();
      }

      /// <summary>
      /// Gets the specific running Navision client instance that corresponds to the supplied serverType/server/database/company.
      /// </summary>
      /// <param name="serverType">The server type.</param>
      /// <param name="server">The server.  If server is an empty string or <c>null</c>, it is ignored</param>
      /// <param name="database">The database.  If database is an empty string or <c>null</c>, it is ignored</param>
      /// <param name="company">The company.  If company is an empty string or <c>null</c>, it is ignored</param>
      /// <remarks>If a client instance already exists it will be returned the way it is, regardless of the useEvents parameter. This means if you wish to be absolutely certain
      /// you should cleanup any existing client instances. Also, if the instance you are trying to bind to is busy it will likely not be found.</remarks>
      /// <returns>The CSide <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"></see> instance corresponding to the server/database/company given</returns>
      public static Client GetClient(ServerType serverType, string server, string database, string company)
      {
         throw new NotImplementedException();
      }
   }
}
