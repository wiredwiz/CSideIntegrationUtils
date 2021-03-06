﻿// <copyright>Copyright 2019 Thaddeus L Ryker</copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// Dynamics Nav is a registered trademark of the Microsoft Corporation

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

using Org.Edgerunner.Dynamics.Nav.CSide.Interfaces;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   ///    Class that acts as a repository of running clients.
   /// </summary>
   public class ClientRepository
   {
      #region Delegates

      /// <summary>
      ///    Delegate that defines a client event handler
      /// </summary>
      /// <param name="sender">The sender.</param>
      /// <param name="client">The client.</param>
      public delegate void ClientEventHandler(object sender, Client client);

      #endregion

      private static ClientRepository _Default;

      /// <summary>
      ///    The synchronization context
      /// </summary>
      internal readonly SynchronizationContext Context;

      private readonly List<long> _ClosingClientIds;

      private readonly Dictionary<long, Client> _RunningClients;

      private bool _PollClients;

      private int _PollingInterval;

      private Thread _PollingThread;

      #region Constructors And Finalizers

      /// <summary>
      ///    Initializes a new instance of the <see cref="ClientRepository" /> class.
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

      #endregion

      /// <summary>
      ///    Gets the default repository instance.
      /// </summary>
      /// <value>The default repository.</value>
      public static ClientRepository Default => _Default ?? (_Default = new ClientRepository());

      /// <summary>
      ///    Gets or sets a value indicating whether to poll for client changes.
      /// </summary>
      /// <value><c>true</c> if polling for client changes; otherwise, <c>false</c>.</value>
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

      /// <summary>
      ///    Gets or sets the polling interval in which to look for client changes.
      /// </summary>
      /// <value>The polling interval.</value>
      /// <exception cref="ArgumentException">Cannot be less than 100 - PollingInterval</exception>
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

      #region Static

      /// <summary>
      ///    Returns a pointer to an implementation of IBindCtx (a bind context object).
      ///    This object stores information about a particular moniker-binding operation.
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="bindingContext">
      ///    Address of an IBindCtx* pointer variable that receives
      ///    the interface pointer to the new bind context object. When the function is
      ///    successful, the caller is responsible for calling Release on the bind context.
      ///    A NULL value for the bind context indicates that an error occurred.
      /// </param>
      [DllImport("ole32.dll")]
      // ReSharper disable once StyleCop.SA1650
      private static extern void CreateBindCtx(int reserved, out IBindCtx bindingContext);

      /// <summary>
      ///    Returns a pointer to the IRunningObjectTable interface on the local running object table (ROT).
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="objectTable">
      ///    The address of an IRunningObjectTable* pointer variable that receives the interface pointer to the local ROT.
      ///    When the function is successful, the caller is responsible for calling Release on the interface pointer. If an error
      ///    occurs, *pprot is undefined.
      /// </param>
      [DllImport("ole32.dll")]
      // ReSharper disable once StyleCop.SA1650
      private static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable objectTable);

      /// <summary>
      ///    Gets the window thread process identifier.
      /// </summary>
      /// <param name="windowHandle">The window handle.</param>
      /// <param name="processId">The process identifier.</param>
      /// <returns>The return value is the identifier of the thread that created the window.</returns>
      [DllImport("user32.dll", SetLastError = true)]
      private static extern uint GetWindowThreadProcessId(IntPtr windowHandle, out uint processId);

      /// <summary>
      ///    Packs the key to identify a client instance.
      /// </summary>
      /// <param name="windowHandle">The window handle.</param>
      /// <param name="processId">The process identifier.</param>
      /// <returns>A System.Int64 that represents the key.</returns>
      private static long PackKey(int windowHandle, uint processId)
      {
         long key = windowHandle;
         key <<= 32;
         key |= processId;
         return key;
      }

      /// <summary>
      ///    Unpacks the key that identifies a client instance.
      /// </summary>
      /// <param name="key">The key.</param>
      /// <param name="windowHandle">The window handle.</param>
      /// <param name="processId">The process identifier.</param>
      private static void UnpackKey(long key, out int windowHandle, out int processId)
      {
         windowHandle = (int)(key >> 32);
         processId = (int)key;
      }

      #endregion

      /// <summary>
      ///    Event that occurs when a client, that the repository is aware of, closes.
      /// </summary>
      public event ClientEventHandler ClientClosed;

      /// <summary>
      ///    Gets the specific running Navision client instance that corresponds to the supplied
      ///    serverType/server/database/company.
      /// </summary>
      /// <param name="serverType">The server type.</param>
      /// <param name="server">The server name.</param>
      /// <param name="database">The database name.</param>
      /// <param name="company">The company.  If company is an empty string or <c>null</c>, it is ignored</param>
      /// <returns>
      ///    The CSide <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"></see> instance corresponding to the
      ///    server/database/company given
      /// </returns>
      /// <exception cref="ArgumentNullException">
      ///    <paramref name="server" /> or <paramref name="database" /> is
      ///    <see langword="null" />
      /// </exception>
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
      ///    Gets the specific Navision client instance by its unique identifier in the repository.
      /// </summary>
      /// <param name="identifier">The identifier.</param>
      /// <returns>The <see cref="Client" /> instance or null if no instance found.</returns>
      public virtual Client GetClientById(long identifier)
      {
         if (!_PollClients)
         {
            var designers = GetActiveClientList();
            UpdateClientCache(designers);
         }

         return _RunningClients.TryGetValue(identifier, out var client) ? client : null;
      }

      /// <summary>
      ///    Gets a list of the current Navision clients.
      /// </summary>
      /// <returns>
      ///    A <see cref="List{T}" /> of <see cref="Client" /> instances corresponding to the currently running nav
      ///    clients.
      /// </returns>
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
      ///    Event that occurs when the repository detects a new client instance.
      /// </summary>
      public event ClientEventHandler NewClientDetected;

      /// <summary>
      ///    Gets the active client list.
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

               if (!string.IsNullOrEmpty(runningObjectName)
                   && runningObjectName.IndexOf("!C/SIDE!navision://client/run?", StringComparison.Ordinal) != -1
                   && runningObjectName.IndexOf("database=", StringComparison.Ordinal) != -1
                   && !clientList.Contains(runningObjectVal)) clientList.Add(runningObjectVal);
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

      /// <summary>
      ///    Gets the running process ids.
      /// </summary>
      /// <returns>A <see cref="List{T}" /> of integers representing process id's.</returns>
      protected virtual List<int> GetRunningProcessIds()
      {
         var clientProcesses = new List<int>();
         foreach (var process in Process.GetProcesses())
            clientProcesses.Add(process.Id);

         return clientProcesses;
      }

      /// <summary>
      ///    Updates the client cache.
      /// </summary>
      /// <param name="clientList">The client list.</param>
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
               var applicationInstance = designer as INSHyperlink;

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

      /// <summary>
      ///    Raises the client closed event.
      /// </summary>
      /// <param name="client">The client.</param>
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

      /// <summary>
      ///    Raises the new client detected event.
      /// </summary>
      /// <param name="client">The client.</param>
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

      private void BeginPolling()
      {
         var start = new ThreadStart(HandlePolling);
         _PollingThread = new Thread(start) { IsBackground = true };
         _PollingThread.Start();
      }

      /// <summary>
      ///    Cleans up designer instance by decrementing its COM reference count.
      /// </summary>
      /// <param name="designer">The designer to clean up.</param>
      private void CleanUpDesignerInstance(IObjectDesigner designer)
      {
         if (designer == null)
            return;

         // decrement designer instance reference count since we are done with it
         Marshal.ReleaseComObject(designer);
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
      ///    Posts the client closed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostClientClosedEvent(object state)
      {
         ClientClosed?.Invoke(this, state as Client);
      }

      /// <summary>
      ///    Posts the new client event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostNewClientDetectedEvent(object state)
      {
         NewClientDetected?.Invoke(this, state as Client);
      }

      private void StopPolling()
      {
         _PollingThread.Join(400);
         _PollingThread = null;
      }
   }
}