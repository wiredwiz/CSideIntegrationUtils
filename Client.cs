// <copyright>Copyright 2010 Thaddeus L Ryker</copyright>
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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

using Org.Edgerunner.Dynamics.Nav.CSide.EventArguments;
using Org.Edgerunner.Dynamics.Nav.CSide.Exceptions;

// ReSharper disable RedundantCast
namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents an instance of a Dynamics Nav client
   /// </summary>
   public class Client : IDisposable
   {
      #region Non-Public Fields (18)

      // The GUID constant is left for reference of those reading the project, but it isn't actually used here.
      // ReSharper disable once UnusedMember.Local
      private const string NavisionClientInterfaceGuid = "50000004-0000-1000-0004-0000836BD2D2";

      private readonly IObjectDesigner _ObjectDesigner;
      private readonly ClientRepository _Repository;

      private Dictionary<NavObjectType, Dictionary<int, object>> _Objects;
      private EventHandler<DatabaseChangedEventArgs> _DatabaseChanged;
      private EventHandler<CompanyChangedEventArgs> _CompanyChanged;
      private EventHandler<ServerChangedEventArgs> _ServerChanged;
      private string _ApplicationVersion;
      private bool _TransactionInProgress;
      
      internal string _PreviousCompany;
      internal string _PreviousDatabase;
      internal ServerType _PreviousServerType;
      internal string _PreviousServer;
      internal bool _PreviousBusyStatus;
      
      #endregion Non-Public Fields

      #region Constructors/Deconstructors (3)

      /// <summary>
      /// Initializes a new instance of the <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client" /> class.
      /// </summary>
      /// <param name="repository">The repository that generated the client instance.</param>
      /// <param name="objectDesigner">The object designer.</param>
      /// <param name="identifier">The unique identifier for the client.</param>
      /// <param name="windowsHandle">The client windows handle.</param>
      /// <param name="processId">The client process identifier.</param>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      internal Client(ClientRepository repository, IObjectDesigner objectDesigner, long identifier, int windowsHandle, int processId)
      {
         _Repository = repository;
         _ObjectDesigner = objectDesigner;
         Identifier = identifier;
         ProcessId = processId;
         WindowHandle = windowsHandle;
         lock (GetSyncObject())
         {
            _ObjectDesigner.GetCompanyName(out var companyName);
            _ObjectDesigner.GetDatabaseName(out var databaseName);
            _ObjectDesigner.GetServerName(out var serverName);
            _ObjectDesigner.GetServerType(out var serverType);
            _ObjectDesigner.GetCSIDEVersion(out var csideVersion);
            _ObjectDesigner.GetApplicationVersion(out var appVersion);
            _PreviousCompany = companyName ?? string.Empty;
            _PreviousDatabase = databaseName ?? string.Empty;
            _PreviousServerType = (ServerType)serverType;
            _PreviousServer = serverName ?? string.Empty;
            CSideVersion = csideVersion;
            _ApplicationVersion = appVersion;
         }
      }

      /// <summary>
      /// Finalizes an instance of the <see cref="Client"/> class. 
      /// Releases unmanaged resources and performs other cleanup operations before the
      /// <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> is reclaimed by garbage collection.
      /// </summary>
      ~Client()
      {
         Dispose(false);
      }

      #endregion Constructors/Deconstructors

      #region Delegates and Events (5)

      /// <summary>
      /// Delegate used for busy status changed events
      /// </summary>
      /// <param name="sender">The sender.</param>
      /// <param name="isBusy">if set to <c>true</c> [is busy].</param>
      public delegate void BusyStatusEventHandler(object sender, bool isBusy);

      #region Events (4)

      /// <summary>
      /// Occurs when the client instance changes company.
      /// </summary>
      public event EventHandler<CompanyChangedEventArgs> CompanyChanged
      {
         add
         {
            if (_CompanyChanged == null)
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }

            _CompanyChanged = (EventHandler<CompanyChangedEventArgs>)Delegate.Combine(_CompanyChanged, value);
         }

         remove
         {
            _CompanyChanged = (EventHandler<CompanyChangedEventArgs>)Delegate.Remove(_CompanyChanged, value);
            if (_CompanyChanged == null)
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when the client instance changes database.
      /// </summary>
      public event EventHandler<DatabaseChangedEventArgs> DatabaseChanged
      {
         add
         {
            if (_DatabaseChanged == null)
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }

            _DatabaseChanged = (EventHandler<DatabaseChangedEventArgs>)Delegate.Combine(_DatabaseChanged, value);
         }

         remove
         {
            _DatabaseChanged = (EventHandler<DatabaseChangedEventArgs>)Delegate.Remove(_DatabaseChanged, value);
            if (_DatabaseChanged == null)
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when the client instance changes server.
      /// </summary>
      public event EventHandler<ServerChangedEventArgs> ServerChanged
      {
         add
         {
            if (_ServerChanged == null)
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }

            _ServerChanged = (EventHandler<ServerChangedEventArgs>)Delegate.Combine(_ServerChanged, value);
         }

         remove
         {
            _ServerChanged = (EventHandler<ServerChangedEventArgs>)Delegate.Remove(_ServerChanged, value);
            if (_ServerChanged == null)
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when [busy status changed].
      /// </summary>
      public event BusyStatusEventHandler BusyStatusChanged;

      #endregion Events

      #endregion Delegates and Events

      /// <summary>
      /// Gets the windows process identifier of the client.
      /// </summary>
      /// <value>The windows process identifier.</value>
      internal int ProcessId { get; }

      /// <summary>
      /// Gets the <see cref="IObjectDesigner"/> instance corresponding to the client.
      /// </summary>
      /// <value>The <see cref="IObjectDesigner"/> instance.</value>
      internal IObjectDesigner Designer => _ObjectDesigner;

      /// <summary>
      /// Gets the unique identifier for the client.
      /// </summary>
      /// <value>The unique identifier.</value>
      public long Identifier { get; }

      #region Locking support

      private object _SyncLock;

      /// <summary>
      /// Gets the sync object to lock upon.
      /// </summary>
      /// <returns>An object to be used for all synchronization locks within the client.</returns>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      internal object GetSyncObject()
      {
         TimeSpan waitPeriod = TimeSpan.Zero;
         return GetSyncObject(waitPeriod);
      }

      /// <summary>
      /// Gets the sync object to lock upon.
      /// </summary>
      /// <param name="timeoutPeriod">The timeout period.</param>
      /// <returns>An object to be used for all synchronization locks within the client.</returns>
      /// <exception cref="CSideException">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      internal object GetSyncObject(TimeSpan timeoutPeriod)
      {
         if (_SyncLock == null)
            _SyncLock = new object();
         DateTime startTime = DateTime.Now;

         // If the client is busy, we wait until it is not, then we return the synchronization lock object.
         while (true)
         {
            if (IsBusy)
               Thread.Sleep(500);
            else
               return _SyncLock;
            if ((timeoutPeriod != TimeSpan.Zero) && ((startTime - DateTime.Now) > timeoutPeriod))
               throw new CSideException("Timed out waiting for synchronization lock");
         }
      }

      /// <summary>
      /// Attempts to get the sync object to lock upon.
      /// </summary>
      /// <param name="timeoutPeriod">The timeout period.</param>
      /// <returns>An object to be used for all synchronization locks within the client or null if the lock could not be obtained.</returns>
      internal object TryGetSyncObject(TimeSpan timeoutPeriod)
      {
         if (_SyncLock == null)
            _SyncLock = new object();
         DateTime startTime = DateTime.Now;

         // If the client is busy, we wait until it is not, then we return the synchronization lock object.
         while (true)
         {
            if (IsBusy)
               Thread.Sleep(500);
            else
               return _SyncLock;
            if ((timeoutPeriod != TimeSpan.Zero) && ((startTime - DateTime.Now) > timeoutPeriod))
               return null;
         }
      }

      /// <summary>
      ///   Gets a value indicating whether the Dynamics Nav client associated with this instance is running.
      /// </summary>
      /// <value>
      ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
      /// </value>
      public bool IsRunning
      {
         get
         {
            // We attempt to fetch the window handle since it should have very little overhead, and if successful we know the client is responding
            try
            {
               // ReSharper disable once SuspiciousTypeConversion.Global
               INSHyperlink app = _ObjectDesigner as INSHyperlink;
               // ReSharper disable once NotAccessedVariable
               int handle;

               // If we can't retrieve an INSHyperlink reference then the likely hood is that the client is no longer valid.
               // In this case we will return false because the client isn't waiting for anything.  Validity issues should be handled elsewhere.
               if (app == null)
                  return false;
               app.GetNavWindowHandle(out handle);
            }
            catch (COMException ex)
            {
               // we received a call rejected or retry later error which means the client is busy
               if ((ex.ErrorCode == CSideError.RPC_E_CALL_REJECTED) ||
                   (ex.ErrorCode == CSideError.RPC_E_SERVERCALL_RETRYLATER))
                  return true;

               return false;
            }
            catch (InvalidComObjectException)
            {
               return false;
            }

            return true;
         }
      }

      /// <summary>
      /// Gets a value indicating whether the Dynamics Nav client associated with instance is busy.
      /// </summary>
      /// <value><c>true</c> if this instance is busy; otherwise, <c>false</c>.</value>
      public bool IsBusy
      {
         get
         {
            bool result = false;

            // We attempt to fetch the window handle since it should have very little overhead, and if successful we know the client is responding
            try
            {
               // ReSharper disable once SuspiciousTypeConversion.Global
               INSHyperlink app = _ObjectDesigner as INSHyperlink;

               // If we can't retrieve an INSHyperlink reference then the likely hood is that the client is no longer valid.
               // In this case we will return false because the client isn't waiting for anything.  Validity issues should be handled elsewhere.
               app?.GetNavWindowHandle(out var handle);
            }
            catch (COMException ex)
            {
               if ((ex.ErrorCode == CSideError.RPC_E_CALL_REJECTED) ||
                   (ex.ErrorCode == CSideError.RPC_E_SERVERCALL_RETRYLATER))
                  result = true;
            }

            _PreviousBusyStatus = result;
            return result;
         }
      }

      #endregion

      #region Other Methods (24)

      // Static Methods (9) 

      /// <summary>
      /// Returns a pointer to an implementation of IBindCtx (a bind context object).
      /// This object stores information about a particular moniker-binding operation.
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="bindContext">Address of an IBindCtx* pointer variable that receives
      /// the interface pointer to the new bind context object. When the function is
      /// successful, the caller is responsible for calling Release on the bind context.
      /// A NULL value for the bind context indicates that an error occurred.</param>
      [DllImport("ole32.dll")]
      // ReSharper disable once StyleCop.SA1650
      private static extern void CreateBindCtx(int reserved, out IBindCtx bindContext);


      /// <summary>
      /// The CreateStreamOnHGlobal function creates a stream object that uses an HGLOBAL memory handle to store the stream contents.
      /// This object is the OLE-provided implementation of the IStream interface.  
      /// The returned stream object supports both reading and writing, is not transacted, and does not support region locking.
      /// The object calls the GlobalReAlloc function to grow the memory block as required
      /// </summary>
      /// <param name="globalMemoryHandle">The global memory handle.</param>
      /// <param name="deleteOnRelease">A value that indicates whether the underlying handle for this stream object should be automatically freed when the stream object is released.
      /// If set to <c>false</c>, the caller must free the hGlobal after the final release.
      /// If set to <c>true</c>, the final release will automatically free the hGlobal parameter.</param>
      /// <param name="newStream">The address of <see cref="System.Runtime.InteropServices.ComTypes.IStream"/>* pointer variable that receives the interface pointer to the new stream object. Its value cannot be <c>null</c>.</param>
      /// <returns>The resulting error code which is 0 on success.</returns>
      [DllImport("OLE32.DLL")]
      // ReSharper disable once StyleCop.SA1650
      private static extern int CreateStreamOnHGlobal(int globalMemoryHandle, bool deleteOnRelease, out IStream newStream);

      /// <summary>
      /// Gets the active client list.
      /// </summary>
      /// <returns>A list of designer objects corresponding to running client instances</returns>
      /// <remarks>If there are multiple instances with the same database and company, only the first is exposed</remarks>
      internal static List<object> GetActiveClientList()
      {
         List<object> clientList = new List<object>();

         IntPtr numFetched = IntPtr.Zero;
         IRunningObjectTable runningObjectTable = null;
         IEnumMoniker monikerEnumerator = null;
         IBindCtx ctx = null;
         IMoniker[] monikers = new IMoniker[1];

         try
         {
            GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();
            int clientNo = 0;
            while (monikerEnumerator.Next(1, monikers, numFetched) == 0)
            {
               CreateBindCtx(0, out ctx);

               string runningObjectName;
               monikers[0].GetDisplayName(ctx, null, out runningObjectName);

               object runningObjectVal;
               runningObjectTable.GetObject(monikers[0], out runningObjectVal);

               if ((runningObjectName.IndexOf("!C/SIDE!navision://client/run?") != -1) &&
                   !clientList.Contains(runningObjectVal))
               {
                  clientNo += 1;
                  clientList.Add(runningObjectVal);
               }
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
      /// Returns a pointer to the IRunningObjectTable interface on the local running object table (ROT).
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="prot">The address of an IRunningObjectTable* pointer variable that receives the interface pointer to the local ROT.
      /// When the function is successful, the caller is responsible for calling Release on the interface pointer. If an error occurs, *pprot is undefined.</param>
      [DllImport("ole32.dll")]
      internal static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

      /// <summary>
      /// Gets the specific <see cref="Org.Edgerunner.Dynamics.Nav.CSide.IObjectDesigner"/> instance that corresponds to the supplied serverType/server/database/company.
      /// </summary>
      /// <param name="serverType">The server type.</param>
      /// <param name="server">The server.</param>
      /// <param name="database">The database.</param>
      /// <param name="company">The company.</param>
      /// <returns>The matching <see cref="Org.Edgerunner.Dynamics.Nav.CSide.IObjectDesigner"/>.</returns>
      internal static IObjectDesigner GetDesigner(ServerType serverType, string server, string database, string company)
      {
         List<object> runningObjects = GetActiveClientList();

         foreach (IObjectDesigner designer in runningObjects)
         {
            if (designer != null)
            {
               try
               {
                  string currentDatabase;
                  int currentServerType;
                  string currentServer;
                  string currentCompany;
                  designer.GetServerType(out currentServerType);
                  designer.GetServerName(out currentServer);
                  designer.GetDatabaseName(out currentDatabase);
                  designer.GetCompanyName(out currentCompany);
                  if (((currentServerType == (int)ServerType.Unknown) || ((currentServerType != (int)ServerType.Unknown) && ((int)serverType == currentServerType))) &&
                     (string.IsNullOrEmpty(server) || (!string.IsNullOrEmpty(currentServer) && (server == currentServer))) &&
                     (string.IsNullOrEmpty(database) || (!string.IsNullOrEmpty(currentDatabase) && (database == currentDatabase))) &&
                     (string.IsNullOrEmpty(company) || (!string.IsNullOrEmpty(currentCompany) && (company == currentCompany))))
                  {
                     return designer;
                  }
               }
               catch (COMException ex)
               {
                  // unresponsive client, so we skip it for now
               }
            }
         }
         return null;
      }

      /// <summary>
      /// Updates the server, database, and company information for the client instance.
      /// </summary>
      internal void UpdateServerDatabaseCompanyInfo()
      {
         try
         {
            // ReSharper disable once ExceptionNotDocumented
            lock (GetSyncObject(TimeSpan.FromMilliseconds(10)))
            {
               _ObjectDesigner.GetCompanyName(out var companyName);
               _ObjectDesigner.GetDatabaseName(out var databaseName);
               _ObjectDesigner.GetServerName(out var serverName);
               _ObjectDesigner.GetServerType(out var serverType);

               if ((ServerType)serverType != _PreviousServerType || serverName != _PreviousServer)
               {
                  var previousServerType = _PreviousServerType;
                  var previousServerName = _PreviousServer;
                  _PreviousServerType = (ServerType)serverType;
                  _PreviousServer = serverName ?? string.Empty;
                  ThreadPool.QueueUserWorkItem(delegate { RaiseServerChanged(new ServerChangedEventArgs(previousServerType, previousServerName, (ServerType)serverType, serverName)); });
               }

               if (databaseName != _PreviousDatabase)
               {
                  var previousDatabase = _PreviousDatabase;
                  _PreviousDatabase = databaseName ?? string.Empty;
                  ThreadPool.QueueUserWorkItem(delegate { RaiseDatabaseChanged(new DatabaseChangedEventArgs(previousDatabase, databaseName)); });
               }

               if (companyName != _PreviousCompany)
               {
                  var previousCompanyName = _PreviousCompany;
                  _PreviousCompany = companyName ?? string.Empty;
                  ThreadPool.QueueUserWorkItem(delegate { RaiseCompanyChanged(new CompanyChangedEventArgs(previousCompanyName, companyName)); });
               }
            }
         }
         catch (CSideException)
         {
            // fail silently in this case
         }
      }

      /// <summary>
      /// Posts the busy status changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostBusyStatusChangedEvent(object state)
      {
         BusyStatusChanged?.Invoke(this, (bool)state);
      }

      /// <summary>
      /// Posts the company changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostCompanyChangedEvent(object state)
      {
         _CompanyChanged(this, state as CompanyChangedEventArgs);
      }

      /// <summary>
      /// Posts the database changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostDatabaseChangedEvent(object state)
      {
         _DatabaseChanged(this, state as DatabaseChangedEventArgs);
      }

      /// <summary>
      /// Posts the server changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostServerChangedEvent(object state)
      {
         _ServerChanged(this, state as ServerChangedEventArgs);
      }

      /// <summary>
      /// Raises the BusyStatusChanged event.
      /// </summary>
      /// <param name="isBusy">if set to <c>true</c> [is busy].</param>
      // ReSharper disable once StyleCop.SA1305
      internal void RaiseBusyStatusChanged(bool isBusy)
      {
         if (BusyStatusChanged != null)
         {
            if (_Repository.Context != null)
               _Repository.Context.Post(PostBusyStatusChangedEvent, isBusy);
            else
               PostBusyStatusChangedEvent(isBusy);
         }
      }

      /// <summary>
      /// Raises the CompanyChanged.
      /// </summary>
      /// <param name="args">The <see cref="CSideEventArgs"/> instance containing the event data.</param>
      internal void RaiseCompanyChanged(CompanyChangedEventArgs args)
      {
         if (_CompanyChanged != null)
         {
            if (_Repository.Context != null)
               _Repository.Context.Post(PostCompanyChangedEvent, args);
            else
               PostCompanyChangedEvent(args);
         }
      }

      /// <summary>
      /// Raises the DatabaseChanged.
      /// </summary>
      /// <param name="args">The <see cref="CSideEventArgs"/> instance containing the event data.</param>
      internal void RaiseDatabaseChanged(DatabaseChangedEventArgs args)
      {
         if (_DatabaseChanged != null)
         {
            if (_Repository.Context != null)
               _Repository.Context.Post(PostDatabaseChangedEvent, args);
            else
               PostDatabaseChangedEvent(args);
         }
      }

      /// <summary>
      /// Raises the ServerChanged event.
      /// </summary>
      /// <param name="args">The <see cref="CSideEventArgs"/> instance containing the event data.</param>
      internal void RaiseServerChanged(ServerChangedEventArgs args)
      {
         if (_ServerChanged != null)
         {
            if (_Repository.Context != null)
               _Repository.Context.Post(PostServerChangedEvent, args);
            else
               PostServerChangedEvent(args);
         }
      }

      /// <summary>
      /// Transfers the contents of a Stream into an IStream.
      /// </summary>
      /// <param name="stream">The <see cref="System.IO.Stream"/>.</param>
      /// <returns>An <see cref="System.Runtime.InteropServices.ComTypes.IStream"/></returns>
      private unsafe IStream ToIStream(Stream stream)
      {
         byte[] buffer = new byte[stream.Length];
         stream.Read(buffer, 0, buffer.Length);
         uint num = 0;
         IntPtr pointerToBytesWritten = new IntPtr((void*)&num);
         CreateStreamOnHGlobal(0, true, out var outStream);
         outStream.Write(buffer, buffer.Length, pointerToBytesWritten);
         outStream.Seek((long)0, 0, IntPtr.Zero);
         return outStream;
      }

      /// <summary>
      /// Transfers the contents of an IStream into a MemoryStream.
      /// </summary>
      /// <param name="comStream">The COM <see cref="System.Runtime.InteropServices.ComTypes.IStream"/>.</param>
      /// <returns>A <see cref="System.IO.MemoryStream"/></returns>
      private unsafe MemoryStream ToMemoryStream(IStream comStream)
      {
         MemoryStream stream = new MemoryStream();
         byte[] pv = new byte[100];
         uint num = 0;
         IntPtr pcbRead = new IntPtr((void*)&num);
         comStream.Seek((long)0, 0, IntPtr.Zero);
         do
         {
            num = 0;
            comStream.Read(pv, pv.Length, pcbRead);
            stream.Write(pv, 0, (int)num);
         }
         while (num > 0);
         return stream;
      }

      /// <summary>
      /// Returns a string that represents the current object.
      /// </summary>
      /// <returns>
      /// A string that represents the current object.
      /// </returns>
      public override string ToString()
      {
         return string.Format(@"{0}\{1}-{2}", Server, Database, Company);
      }

      #endregion Methods

      #region IObjectDesigner functionality

      /// <summary>
      /// Compiles the specified object.
      /// </summary>
      /// <param name="navObjectType">Type of the Nav object.</param>
      /// <param name="objectId">The object Id.</param>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">Either the client is busy, invalid or there was a license permission issue.</exception>
      public void CompileObject(NavObjectType navObjectType, int objectId)
      {
         lock (GetSyncObject())
         {
            int result = _ObjectDesigner.CompileObject((int)navObjectType, objectId);
            if (result != 0)
               throw CSideException.GetException(result);
         }
      }

      /// <summary>
      /// Compiles the objects within the supplied filter.
      /// </summary>
      /// <param name="filter">The filter.</param>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">Either the client is busy, invalid or there was a license permission issue.</exception>
      public void CompileObjects(string filter)
      {
         lock (GetSyncObject())
         {
            int result = _ObjectDesigner.CompileObjects(filter);
            if (result != 0)
               throw CSideException.GetException(result);
         }
      }

      /// <summary>
      /// Gets the company name.
      /// </summary>
      /// <value>The company.</value>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException" accessor="get">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      public string Company
      {
         get
         {
            if (IsBusy)
               return _PreviousCompany;

            lock (GetSyncObject())
            {
               _ObjectDesigner.GetCompanyName(out var companyName);
               companyName = companyName ?? string.Empty;
               if (companyName != _PreviousCompany)
               {
                  var previousCompany = _PreviousCompany;
                  _PreviousCompany = companyName;
                  ThreadPool.QueueUserWorkItem(delegate { RaiseCompanyChanged(new CompanyChangedEventArgs(previousCompany, companyName)); });
               }

               return _PreviousCompany;
            }
         }
      }

      /// <summary>
      /// Gets the database name.
      /// </summary>
      /// <value>The database.</value>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException" accessor="get">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      public string Database
      {
         get
         {
            if (IsBusy)
               return _PreviousDatabase;

            lock (GetSyncObject())
            {
               _ObjectDesigner.GetDatabaseName(out var databaseName);
               databaseName = databaseName ?? string.Empty;
               if (databaseName != _PreviousDatabase)
               {
                  var previousDatabase = _PreviousDatabase;
                  _PreviousDatabase = databaseName;
                  ThreadPool.QueueUserWorkItem(delegate { RaiseDatabaseChanged(new DatabaseChangedEventArgs(previousDatabase, databaseName)); });
               }

               return _PreviousDatabase;
            }
         }
      }

      /// <summary>
      /// Gets the server name.
      /// </summary>
      /// <value>The server.</value>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException" accessor="get">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      public string Server
      {
         get
         {
            if (IsBusy)
               return _PreviousServer;

            lock (GetSyncObject())
            {
               _ObjectDesigner.GetServerName(out var serverName);
               serverName = serverName ?? string.Empty;
               if (serverName != _PreviousServer)
               {
                  var previousServerName = _PreviousServer;
                  _PreviousServer = serverName;
                  _ObjectDesigner.GetServerType(out var serverType);
                  var previousServerType = _PreviousServerType;
                  _PreviousServerType = (ServerType)serverType;
                  ThreadPool.QueueUserWorkItem(delegate { RaiseServerChanged(new ServerChangedEventArgs(previousServerType, previousServerName, (ServerType)serverType, serverName)); });
               }

               return _PreviousServer;
            }
         }
      }

      /// <summary>
      /// Gets the type of the server.
      /// </summary>
      /// <value>The type of the server.</value>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException" accessor="get">Thrown if the timeoutPeriod expires and the client is still busy.</exception>
      public ServerType ServerType
      {
         get
         {
            if (IsBusy)
               return _PreviousServerType;

            lock (GetSyncObject())
            {
               _ObjectDesigner.GetServerType(out var serverType);
               _PreviousServerType = (ServerType)serverType;
               if ((ServerType)serverType != _PreviousServerType)
               {
                  var previousServerType = _PreviousServerType;
                  _PreviousServerType = (ServerType)serverType;
                  _ObjectDesigner.GetServerName(out var serverName);
                  serverName = serverName ?? string.Empty;
                  var previousServerName = _PreviousServer;
                  _PreviousServer = serverName;
                  ThreadPool.QueueUserWorkItem(delegate
                  {
                     RaiseServerChanged(new ServerChangedEventArgs(previousServerType, previousServerName, (ServerType)serverType, serverName));
                  });
               }

               return _PreviousServerType;
            }
         }
      }

      /// <summary>
      /// Gets the C/Side version.
      /// </summary>
      /// <value>The C/Side version.</value>
      public string CSideVersion { get; }

      /// <summary>
      /// Gets the application version.
      /// </summary>
      /// <value>The application version.</value>
      public string ApplicationVersion
      {
         get
         {
            if (IsBusy)
               return _ApplicationVersion;

            lock (GetSyncObject())
            {
               string appVersion;
               int result = _ObjectDesigner.GetApplicationVersion(out appVersion);
               if (result != 0)
                  throw CSideException.GetException(result);
               _ApplicationVersion = appVersion ?? string.Empty;
               return appVersion;
            }
         }
      }

      /// <summary>
      /// Reads the specified object to a stream in its text format.
      /// </summary>
      /// <param name="navObjectType">Type of the Navision object.</param>
      /// <param name="objectId">The object Id.</param>
      /// <returns>A <see cref="System.IO.MemoryStream"/> containing the text for the specified object</returns>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">Either the client is busy, invalid or there was a license permission issue.</exception>
      public MemoryStream ReadObjectToStream(NavObjectType navObjectType, int objectId)
      {
         lock (GetSyncObject())
         {
            CreateStreamOnHGlobal(0, true, out var outStream);

            // We Use ReadObjects() here instead of ReadObject() because ReadObject() is very buggy and outputs bad files
            int result = _ObjectDesigner.ReadObjects(
               string.Format(
                  "WHERE(Type=CONST({0}),ID=CONST({1}))", 
                  (int)navObjectType, 
                  objectId), 
               outStream);

            if (result != 0)
               throw CSideException.GetException(result);

            return ToMemoryStream(outStream);
         }
      }

      /// <summary>
      /// Reads the specified objects to a stream in their text format.
      /// </summary>
      /// <param name="navObjectType">Type of the Navision object.</param>
      /// <param name="objectIdRangeStart">The object number to read from.</param>
      /// <param name="objectIdRangeStop">The object number to read to.</param>
      /// <returns>A <see cref="System.IO.MemoryStream" /> containing the text for the specified objects</returns>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">An error occurred while attempting the read.  A likely cause is a license permission issue.</exception>
      public MemoryStream ReadObjectsToStream(NavObjectType navObjectType, int objectIdRangeStart, int objectIdRangeStop)
      {
         lock (GetSyncObject())
         {
            CreateStreamOnHGlobal(0, true, out var outStream);

            // We Use ReadObjects() here instead of ReadObject() because ReadObject() is very buggy and outputs bad files
            int result = _ObjectDesigner.ReadObjects(
               string.Format(
                  "WHERE(Type=CONST({0}),ID=FILTER({1}..{2}))", 
                  (int)navObjectType, 
                  objectIdRangeStart, 
                  objectIdRangeStop), 
               outStream);

            if (result != 0)
               throw CSideException.GetException(result);

            return ToMemoryStream(outStream);
         }
      }

      /// <summary>
      /// Writes the specified text object(s) to Navision from a stream.
      /// </summary>
      /// <param name="stream">The stream containing the text for a Navision object.</param>
      /// <remarks>The stream may contain one or more text objects to be written to the database.</remarks>
      /// <exception cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException">An error occurred while attempting the read.  A likely cause is a license permission issue.</exception>
      public void WriteObjectsFromStream(Stream stream)
      {
         lock (GetSyncObject())
         {
            IStream source = ToIStream(stream);
            int result = _ObjectDesigner.WriteObjects(source);
            if (result != 0)
               throw CSideException.GetException(result);
         }
      }
      #endregion

      #region INSAppBase functionality

      /// <summary>
      /// Gets the specified table.
      /// </summary>
      /// <param name="tableNo">The table ID.</param>
      /// <returns>An instance of a <see cref="Org.Edgerunner.Dynamics.Nav.CSide.INSTable"/> or <c>null</c> if unable to get a reference</returns>
      internal INSTable GetTableInternal(int tableID)
      {
         lock (GetSyncObject())
         {
            INSTable table;
            INSAppBase appBase = _ObjectDesigner as INSAppBase;
            if (appBase == null)
               return null;
            appBase.GetTable(tableID, out table);
            return table;
         }
      }

      /// <summary>
      /// Enumerates the tables in the database the client is attached to.
      /// </summary>
      /// <param name="enumerator">The enumerator.</param>
      /// <param name="tableID">The table ID.</param>
      internal void EnumTables(CallbackEnumerator enumerator, int tableID)
      {
         lock (GetSyncObject())
         {
            INSAppBase appBase = _ObjectDesigner as INSAppBase;
            if (appBase == null)
               return;
            appBase.EnumTables(enumerator, tableID);
         }
      }


      /// <summary>
      /// Retrieves an Table instance that corresponds to the table ID provided.
      /// </summary>
      /// <param name="tableID">The table ID.</param>
      /// <returns>A <see cref="Table"/> instance</returns>
      /// <remarks>Attempts to retrieve the table name via EnumTables(), but some table names cannot be obtained this way.
      /// This is usually only for some virtual tables.  This method will first attempt to lookup the name based on known virtual tables.
      /// If the name cannot be retrieved this way, then it is set to "[Name Unavailable]"</remarks>
      public Table GetTable(int tableID)
      {
         Table result;
         INSTable backingTable;
         INSAppBase appBase = _ObjectDesigner as INSAppBase;
         if (appBase == null)
            return null;
         CallbackEnumerator cbEnum = new CallbackEnumerator(this);
         EnumTables(cbEnum, tableID);
         Dictionary<int, Table> tables = cbEnum.Tables;
         if (tables.Count != 0)
            result = tables[tableID];
         else
         {
            result = new Table(tableID, VirtualTables.TryGetName(tableID) ?? "[Name Unavailable]", this);
            int error = appBase.GetTable(tableID, out backingTable);
            if (error != 0)
               return null;  // maybe this should throw an exception instead
            result.SetBackingTable(backingTable);
         }
         return result;

      }

      /// <summary>
      /// Begins a transaction inside the Navision client instance.
      /// </summary>
      public void BeginTransaction()
      {
         lock (GetSyncObject())
         {
            INSAppBase appBase = _ObjectDesigner as INSAppBase;
            if (appBase == null)
               return;
            if (_TransactionInProgress)
               throw new CSideException("A transaction is already in progress.");
            int result = appBase.StartTrans();
            if (result != 0)
               throw CSideException.GetException(result);
            _TransactionInProgress = true;
         }
      }

      /// <summary>
      /// Triggers an error to be displayed inside the Navision client instance with the supplied message.
      /// </summary>
      /// <param name="message">The message to be displayed.</param>
      public void Error(string message)
      {
         lock (GetSyncObject())
         {
            INSAppBase appBase = _ObjectDesigner as INSAppBase;
            if (appBase == null)
               return;
            try
            {
               int result = appBase.Error(message);
               if (result != 0)
                  throw CSideException.GetException(result);
            }
            catch (COMException ex)
            {
               if (ex.Message != message)
                  throw ex;
            }
         }
      }

      /// <summary>
      /// Ends current the transaction.
      /// </summary>
      /// <param name="commitChanges">if set to <c>true</c> any pending modifications are committed, else they are discarded.</param>
      public void EndTransaction(bool commitChanges)
      {
         lock (GetSyncObject())
         {
            _TransactionInProgress = false;
            INSAppBase appBase = _ObjectDesigner as INSAppBase;
            if (appBase == null)
               return;
            appBase.EndTransaction(commitChanges);
         }
      }

      /// <summary>
      /// Commits the current transaction and automatically begins a new one.
      /// </summary>
      /// <remarks>If you need to simply commit the current changes during a transaction, this method should be used rather than EndTransaction</remarks>
      public void Commit()
      {
         bool inProgress = _TransactionInProgress;
         EndTransaction(true);
         if (inProgress)
            BeginTransaction();
      }

      /// <summary>
      /// Gets the tables in the database the client is attached.
      /// </summary>
      /// <remarks>The tables returned will include some (but not all) of the virtual tables in the database. Some virtual tables that this will not include can still be obtained
      /// with a call to GetTable().</remarks>
      /// <value>The tables.</value>
      public Dictionary<int, Table> Tables
      {
         get
         {
            // I toyed with adding caching logic here to lower the overhead of repeated fetches but there were too many
            // problems with responding to table deletes/additions/changes.  Might add something later Due to this overhead.
            // It is recommended that you use FetchTable() to get a specific table.
            lock (GetSyncObject())
            {
               INSAppBase appBase = _ObjectDesigner as INSAppBase;
               if (appBase == null)
                  return null;
               CallbackEnumerator cbEnum = new CallbackEnumerator(this);
               appBase.EnumTables(cbEnum, 0);
               return cbEnum.Tables;
            }
         }
      }

      /// <summary>
      /// Gets the objects in the database the client is attached to.
      /// </summary>
      /// <value>The objects.</value>
      public Dictionary<NavObjectType, Dictionary<int, object>> Objects
      {
         get
         {
            lock (GetSyncObject())
            {
               if (_Objects == null)
               {
                  // first we fetch the Object table
                  Table table = GetTable(2000000001);
                  if (table == null)
                     throw new CSideException("Unable to retrieve the Object table");
                  // Filter for blank company and objects of type other than tabledata
                  table.SetFilter(1, ">0");
                  table.SetFilter(2, "=''");
                  // I'm leaving the below code as is instead of using LINQ for the readability of those new to C#
                  List<Record> records = table.FetchRecords();
                  _Objects = new Dictionary<NavObjectType, Dictionary<int, object>>();
                  foreach (NavObjectType objectType in Enum.GetValues(typeof(NavObjectType)))
                     _Objects.Add(objectType, new Dictionary<int, object>());
                  foreach (Record record in records)
                  {
                     Object nObject = new Object(record);
                     _Objects[nObject.Type].Add(nObject.ID, nObject); ;
                  }
               }
               return _Objects;
            }
         }
      }

      /// <summary>Retrieves a list of <see cref="Object"/>(s)</summary>
      /// <param name="objectType">The object type to be retrieved</param>
      /// <param name="objectID">The ID number of the object to retrieve</param>
      /// <remarks>If you wish to retrieve all objects of a given type, the <see cref="objectID"/> should be 0.</remarks>
      /// <returns>A List of objects</returns>
      private List<Object> DoGetObjects(NavObjectType objectType, int objectID)
      {
         lock (GetSyncObject())
         {
            // first we fetch the Object table
            var table = GetTable(2000000001);
            if (table == null)
               throw new CSideException("Unable to retrieve the Object table");
            // Filter for the type of object we are fetching
            table.SetFilter(1, string.Format("={0}", (int)objectType));
            table.SetFilter(2, "=''");
            // Filter for the specific object number if it was specified
            if (objectID != 0)
               table.SetFilter(3, string.Format("={0}", objectID));
            return table.FetchRecords().ConvertAll<Object>(x => new Object(x));
         }
      }

      /// <summary>Retrieves a specific Object instance.</summary>
      /// <param name="objectType">Object type you wish to retrieve.</param>
      /// <param name="objectID">ID number of the object you wish to retrieve.</param>
      /// <returns>An Object instance.</returns>
      public Object GetObject(NavObjectType objectType, int objectID)
      {
         return DoGetObjects(objectType, objectID).FirstOrDefault<Object>();
      }

      /// <summary>Retrieves a dictionary containing objects indexed by their object number.</summary>
      /// <param name="objectType">Object type you wish to retreive.</param>
      /// <returns>A dictionary of objects indexed by their number.</returns>
      public Dictionary<int, Object> GetObjects(NavObjectType objectType)
      {
         return DoGetObjects(objectType, 0).ToDictionary<Object, int>(o => o.ID);
      }
      #endregion

      #region INSHyperlink functionality

      /// <summary>
      /// Opens the hyperlink within the Navision client instance.
      /// </summary>
      /// <param name="link">The hyperlink.</param>
      /// <remarks>This hyperlink should be a navision hyperlink (begins with navision://client/run?). It may contain instructions to open a specific form or report as well as a
      /// pointer to a specific record.  You may use the <see cref="ClientLink"/> class to easily construct valid client links.</remarks>
      public void OpenLink(string link)
      {
         lock (GetSyncObject())
         {
            if (string.IsNullOrEmpty(link))
               return;
            INSHyperlink app = _ObjectDesigner as INSHyperlink;
            if (app != null)
               app.Open(link);
         }
      }

      /// <summary>
      /// Gets the window handle for the Navision client instance.
      /// </summary>
      /// <value>The window handle.</value>
      public int WindowHandle { get; }

      #endregion

      #region INSApplication functionality

      /// <summary>
      /// Gets the current open and active form inside the Navision client instance.
      /// </summary>
      /// <value>The current <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Form"/>.</value>
      public Form CurrentForm
      {
         get
         {
            lock (GetSyncObject())
            {
               INSApplication app = _ObjectDesigner as INSApplication;
               if (app == null)
                  return null;
               INSForm form;
               int result = app.GetCurrentForm(out form);
               if (result != 0)
                  throw CSideException.GetException(result);
               return new Form(this, form);
            }
         }
      }
      #endregion

      #region IDisposable Members

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Releases unmanaged and - optionally - managed resources
      /// </summary>
      /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            if (_Objects != null)
               foreach (NavObjectType objectType in _Objects.Keys)
                  foreach (Object navObject in _Objects[objectType].Values)
                     navObject.Dispose();
         }
         // free unmanaged resources
         if (_ObjectDesigner != null)
            Marshal.ReleaseComObject(_ObjectDesigner);
      }
      #endregion
   }
}

