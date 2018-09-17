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
      private readonly List<long> _ClosingClientIds;
      internal readonly SynchronizationContext Context;
      private Thread _PollingThread;
      private int _PollingInterval;
      private bool _PollClients;

      /// <summary>
      /// Initializes a new instance of the <see cref="ClientRepository"/> class.
      /// </summary>
      public ClientRepository()
      {
         _PollClients = true;
         _PollingInterval = 200;
         Context = SynchronizationContext.Current;
         _ClosingClientIds = new List<long>();
         _RunningClients = new Dictionary<long, Client>();
         BeginPolling();
      }

      ~ClientRepository()
      {
         StopPolling();
      }

      public static ClientRepository Default => _Default ?? (_Default = new ClientRepository());


      public delegate void ClientEventHandler(object sender, Client client);

      /// <summary>
      /// Occurs when the repository detects a new client instance.
      /// </summary>
      public event ClientEventHandler NewClientDetected;

      /// <summary>
      /// Occurs when a client, that the repository is aware of, closes.
      /// </summary>
      public event ClientEventHandler ClientClosed;

      public int PollingInterval
      {
         get => _PollingInterval;
         set
         {
            if (value < 100)
               throw new ArgumentException("Cannot be less than 100", nameof(PollingInterval));

            _PollingInterval = value;
         }
      }

      public bool PollClients
      {
         get => _PollClients;
         set
         {
            _PollClients = value;
            if (_PollClients)
               BeginPolling();
            else
               StopPolling();
         }
      }

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

      /// <summary>
      /// Posts the client closed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostClientClosedEvent(object state)
      {
         ClientClosed?.Invoke(this, state as Client);
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

      private void BeginPolling()
      {
         var start = new ThreadStart(HandlePolling);
         _PollingThread = new Thread(start) { IsBackground = true };
         _PollingThread.Start();
      }

      private void StopPolling()
      {
         _PollingThread.Join(400);
         _PollingThread = null;
      }

      private void HandlePolling()
      {
         while (_PollClients)
         {
            var designers = GetActiveClientList();
            UpdateClientCache(designers);
            Thread.Sleep(_PollingInterval); 
         }
      }

      /// <summary>
      /// Cleans up designer instance by decrementing its COM reference count.
      /// </summary>
      /// <param name="designer">The designer to clean up.</param>
      private void CleanUpDesignerInstance(IObjectDesigner designer)
      {
         if (designer == null)
            return;

         // decrement designer instance reference count since we are done with it
         Marshal.ReleaseComObject(designer);
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
         // Clear the list of closing client keys from last poll, since they should now be closed
         _ClosingClientIds.Clear();

         // First remove all dead clients from our list by checking whether their process is alive
         var pids = GetRunningProcessIds();
         foreach (var pair in _RunningClients.ToList())
         {
            var client = pair.Value;
            if (!pids.Contains(pair.Value.ProcessId))
            {               
               _RunningClients.Remove(pair.Key);
               _ClosingClientIds.Add(pair.Key);
               ThreadPool.QueueUserWorkItem(delegate { RaiseClientClosed(client); });               
            }
            else
            {
               var previous = client._PreviousBusyStatus;
               var current = client.IsBusy;
               if (previous != current)
                  ThreadPool.QueueUserWorkItem(delegate { client.RaiseBusyStatusChanged(current); });
            }
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
               {
                  CleanUpDesignerInstance(designer);
                  continue;
               }
               applicationInstance.GetNavWindowHandle(out handle);
            }
            catch (COMException)
            {
               CleanUpDesignerInstance(designer);
               // The client is likely busy, in which case we are just going to come back to it once it is responding
               continue;
            }
            catch (Exception)
            {
               CleanUpDesignerInstance(designer);
               // Some other unknown issue is going on with this client, so we will skip it this pass
               continue;
            }

            // If the client just closed and the handle is no longer valid, we skip it
            if (GetWindowThreadProcessId((IntPtr)handle, out var pid) == 0)
            {
               CleanUpDesignerInstance(designer);
               continue;
            }

            var key = PackKey(handle, pid);

            // Make sure this "new" client we are seeing is not a client in the middle of closing that we just removed from our list
            // If the instance is a closing client then we skip it
            if (_ClosingClientIds.Contains(key))
            {
               CleanUpDesignerInstance(designer);
               continue;
            }

            if (!_RunningClients.TryGetValue(key, out var client))
            {
               // we do not release the designer reference here since the new client instance will be in charge of doing that
               client = new Client(this, designer, key, handle, (int)pid);
               _RunningClients[key] = client;
               ThreadPool.QueueUserWorkItem(delegate { RaiseNewClientDetected(client); });               
            }
            else
            {
               CleanUpDesignerInstance(designer);
               // update existing client data
               client.UpdateServerDatabaseCompanyInfo();
            }
         }
      }

      protected virtual List<int> GetRunningProcessIds()
      {
         var clientProcesses = new List<int>();
         foreach (Process process in Process.GetProcesses())
         {
            //if (string.Compare(process.ProcessName, "finsql", StringComparison.OrdinalIgnoreCase) == 0 ||
            //    string.Compare(process.ProcessName, "fin", StringComparison.OrdinalIgnoreCase) == 0)
               clientProcesses.Add(process.Id);
         }

         return clientProcesses;
      }

      internal void RaiseNewClientDetected(Client client)
      {
         if (NewClientDetected != null)
         {
            if (Context != null)
               Context.Post(PostNewClientDetectedEvent, client);
            else
               PostNewClientDetectedEvent(client);
         }
      }

      internal void RaiseClientClosed(Client client)
      {
         if (ClientClosed != null)
         {
            if (Context != null)
               Context.Post(PostClientClosedEvent, client);
            else
               PostClientClosedEvent(client);
         }
      }

      public virtual List<Client> GetClients()
      {
         if (!_PollClients)
         {
            var designers = GetActiveClientList();
            UpdateClientCache(designers);
         }
         return _RunningClients?.Values.ToList();
      }

      /// <summary>
      /// Gets the specific running Navision client instance that corresponds to the supplied serverType/server/database/company.
      /// </summary>
      /// <param name="serverType">The server type.</param>
      /// <param name="server">The server name.</param>
      /// <param name="database">The database name.</param>
      /// <param name="company">The company.  If company is an empty string or <c>null</c>, it is ignored</param>
      /// <returns>The CSide <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"></see> instance corresponding to the server/database/company given</returns>
      /// <exception cref="ArgumentNullException"><paramref name="server"/> or <paramref name="database"/> is <see langword="null"/></exception>
      public virtual Client GetClient(ServerType serverType, string server, string database, string company)
      {
         if (string.IsNullOrEmpty(server))
            throw new ArgumentNullException(nameof(server));
         if (string.IsNullOrEmpty(database))
            throw new ArgumentNullException(nameof(database));

         if (!_PollClients)
         {
            var designers = GetActiveClientList();
            UpdateClientCache(designers);
         }
         foreach (var client in _RunningClients.Values)
         {
            if (serverType != client.ServerType)
               continue;
            if (!string.IsNullOrEmpty(company) && company != client.Company)
               continue;
            if (server != client.Server)
               continue;
            if (database != client.Database)
               continue;
            
            return client;
         }

         return null;
      }

      /// <summary>
      /// Gets the specific Navision client instance by its unique identifier in the repository.
      /// </summary>
      /// <param name="identifier">The identifier.</param>
      /// <returns>The <see cref="Client"/> instance or null if no instance found.</returns>
      public virtual Client GetClientById(long identifier)
      {
         if (!_PollClients)
         {
            var designers = GetActiveClientList();
            UpdateClientCache(designers);
         }
         return _RunningClients.TryGetValue(identifier, out var client) ? client : null;
      }
   }
}
