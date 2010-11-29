//
// Copyright 2010 Thaddeus L Ryker
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
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents an instance of a Dynamics Nav client
   /// </summary>
   public class Client : IDisposable
   {
      #region Non-Public Fields (16)

      private SynchronizationContext _Context;
      private EventHandler<CSideEventArgs> _Deactivated;
      private EventHandler<CSideEventArgs> _FormOpened;
      private IObjectDesigner _objectDesigner;
      private Dictionary<NavObjectType, Dictionary<int, Object>> _Objects;
      private ApplicationEventSubscriber _Subscriber;
      private const string NavisionClientInterfaceGUID = "50000004-0000-1000-0004-0000836BD2D2";
      private static Hashtable ObjectMap;
      private EventHandler<CSideEventArgs> _Activated;
      private EventHandler<CSideEventArgs> _DatabaseChanged;
      private EventHandler<CSideEventArgs> _CompanyChanged;
      private EventHandler<CSideEventArgs> _ServerChanged;
      internal string PreviousCompany;
      internal string PreviousDatabase;
      internal ServerType PreviousServerType;
      internal string PreviousServer;

      #endregion Non-Public Fields

      #region Constructors/Deconstructors (3)


      /// <summary>
      /// Initializes a new instance of the <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> class.
      /// </summary>
      /// <param name="objectDesigner">The object designer.</param>
      internal Client(IObjectDesigner objectDesigner)
      {
         _objectDesigner = objectDesigner;
         PreviousCompany = Company;
         PreviousDatabase = Database;
         PreviousServerType = ServerType;
         PreviousServer = Server;
         _Context = SynchronizationContext.Current;
         ConnectApplicationEvents();
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> class.
      /// </summary>
      static Client()
      {
         ObjectMap = new Hashtable();
      }


      /// <summary>
      /// Releases unmanaged resources and performs other cleanup operations before the
      /// <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> is reclaimed by garbage collection.
      /// </summary>
      ~Client()
      {
         Dispose(false);
      }

      #endregion Constructors/Deconstructors

      #region Delegates and Events (6)

      #region Events (6)

      /// <summary>
      /// Occurs when the client instance is [activated].
      /// </summary>
      public event EventHandler<CSideEventArgs> Activated
      {
         add
         {
            if (_Activated == null)		// First listener...
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }
            _Activated = (EventHandler<CSideEventArgs>)Delegate.Combine(_Activated, value);
         }
         remove
         {
            _Activated = (EventHandler<CSideEventArgs>)Delegate.Remove(_Activated, value);
            if (_Activated == null)  // No more listeners to this event
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when [company changed].
      /// </summary>
      public event EventHandler<CSideEventArgs> CompanyChanged
      {
         add
         {
            if (_CompanyChanged == null)		// First listener...
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }
            _CompanyChanged = (EventHandler<CSideEventArgs>)Delegate.Combine(_CompanyChanged, value);
         }
         remove
         {
            _CompanyChanged = (EventHandler<CSideEventArgs>)Delegate.Remove(_CompanyChanged, value);
            if (_CompanyChanged == null)  // No more listeners to this event
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when [database changed].
      /// </summary>
      public event EventHandler<CSideEventArgs> DatabaseChanged
      {
         add
         {
            if (_DatabaseChanged == null)		// First listener...
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }
            _DatabaseChanged = (EventHandler<CSideEventArgs>)Delegate.Combine(_DatabaseChanged, value);
         }
         remove
         {
            _DatabaseChanged = (EventHandler<CSideEventArgs>)Delegate.Remove(_DatabaseChanged, value);
            if (_DatabaseChanged == null)  // No more listeners to this event
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when the client instance is [deactivated].
      /// </summary>
      public event EventHandler<CSideEventArgs> Deactivated
      {
         add
         {
            if (_Deactivated == null)		// First listener...
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }
            _Deactivated = (EventHandler<CSideEventArgs>)Delegate.Combine(_Deactivated, value);
         }
         remove
         {
            _Deactivated = (EventHandler<CSideEventArgs>)Delegate.Remove(_Deactivated, value);
            if (_Deactivated == null)  // No more listeners to this event
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      /// <summary>
      /// Occurs when a form is opened inside the client instance.
      /// </summary>
      public event EventHandler<CSideEventArgs> FormOpened
      {
         add
         {
            if (_FormOpened == null)		// First listener...
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }
            _FormOpened = (EventHandler<CSideEventArgs>)Delegate.Combine(_FormOpened, value);
         }
         remove
         {
            _FormOpened = (EventHandler<CSideEventArgs>)Delegate.Remove(_FormOpened, value);
            if (_FormOpened == null)  // No more listeners to this event
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      public event EventHandler<CSideEventArgs> ServerChanged
      {
         add
         {
            if (_ServerChanged == null)		// First listener...
            {
               // TODO: If needed, add code to respond to the first event hook-up.
            }
            _ServerChanged = (EventHandler<CSideEventArgs>)Delegate.Combine(_ServerChanged, value);
         }
         remove
         {
            _ServerChanged = (EventHandler<CSideEventArgs>)Delegate.Remove(_ServerChanged, value);
            if (_ServerChanged == null)  // No more listeners to this event
            {
               // TODO: Add code to clean up if necessary.
            }
         }
      }

      #endregion Events

      #endregion Delegates and Events

      #region Other Methods (22)

      // Static Methods (7) 

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
      /// The CreateStreamOnHGlobalfunction creates a stream object that uses an HGLOBAL memory handle to store the stream contents.
      /// This object is the OLE-provided implementation of the IStream interface.  
      /// The returned stream object supports both reading and writing, is not transacted, and does not support region locking.
      /// The object calls the GlobalReAlloc function to grow the memory block as required
      /// </summary>
      /// <param name="hGlobalMemHandle">The global memory handle.</param>
      /// <param name="fDeleteOnRelease">A value that indicates whether the underlying handle for this stream object should be automatically freed when the stream object is released.
      /// If set to <c>false</c>, the caller must free the hGlobal after the final release.
      /// If set to <c>true</c>, the final release will automatically free the hGlobal parameter.</param>
      /// <param name="ppStm">The address of <see cref="System.Runtime.InteropServices.ComTypes.IStream"/>* pointer variable that receives the interface pointer to the new stream object. Its value cannot be <c>null</c>.</param>
      /// <returns></returns>
      [DllImport("OLE32.DLL")]
      private static extern int CreateStreamOnHGlobal(int hGlobalMemHandle, bool fDeleteOnRelease, out IStream ppStm);

      /// <summary>
      /// Gets the active client list.
      /// </summary>
      /// <returns>A list of designer objects corresponding to running client instances</returns>
      /// <remarks>If there are multiple instances with the same database and company, only the first is exposed</remarks>
      protected static List<object> GetActiveClientList()
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
      /// Gets the current running Navision client instances.
      /// </summary>
      /// <returns>A List of CSide <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/>s</returns>
      public static List<Client> GetClients()
      {
         List<object> runningObjects = GetActiveClientList();
         List<Client> sessions = new List<Client>();

         foreach (IObjectDesigner designer in runningObjects)
         {
            if (designer != null)
            {
               Client client = GetClientWrapper(designer);
               sessions.Add(client);
            }
         }
         return sessions;
      }

      /// <summary>
      /// Gets the current client wrapper for a given designer instance.  If one doesn't exist, a new one is created.
      /// </summary>
      /// <param name="designer">The running object table designer instance to get a wrapper for.</param>
      /// <returns>A CSide <see cref="Client"/></returns>
      private static Client GetClientWrapper(IObjectDesigner designer)
      {
         Client client = ObjectMap[designer] as Client;
         if (client == null)
         {
            client = new Client(designer);
            ObjectMap[designer] = client;
         }
         return client;
      }

      /// <summary>
      /// Returns a pointer to the IRunningObjectTable interface on the local running object table (ROT).
      /// </summary>
      /// <param name="reserved">This parameter is reserved and must be 0.</param>
      /// <param name="prot">The address of an IRunningObjectTable* pointer variable that receives the interface pointer to the local ROT.
      /// When the function is successful, the caller is responsible for calling Release on the interface pointer. If an error occurs, *pprot is undefined.</param>
      [DllImport("ole32.dll")]
      protected static extern void GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

      /// <summary>
      /// Gets the specific running Navision client instance that corresponds to the supplied server/database/company.
      /// </summary>
      /// <param name="server">The server.  If server is an empty string or <c>null</c>, it is ignored</param>
      /// <param name="database">The database.  If database is an empty string or <c>null</c>, it is ignored</param>
      /// <param name="company">The company.  If company is an empty string or <c>null</c>, it is ignored</param>
      /// <returns>The CSide <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> instance corresponding to the server/database/company given</returns>
      public static Client GetSpecificClient(string server, string database, string company)
      {
         List<object> runningObjects = GetActiveClientList();
         Client client = null;

         foreach (IObjectDesigner designer in runningObjects)
         {
            if (designer != null)
            {
               string currentDatabase;
               string currentServer;
               string currentCompany;
               designer.GetServerName(out currentServer);
               designer.GetDatabaseName(out currentDatabase);
               designer.GetCompanyName(out currentCompany);
               if ((string.IsNullOrEmpty(server) || (!string.IsNullOrEmpty(currentServer) && (server == currentServer))) &&
                  (string.IsNullOrEmpty(database) || (!string.IsNullOrEmpty(currentDatabase) && (database == currentDatabase))) &&
                  (string.IsNullOrEmpty(company) || (!string.IsNullOrEmpty(currentCompany) && (company == currentCompany))))
               {
                  client = GetClientWrapper(designer);
                  break;
               }
            }
         }
         return client;
      }

      // Private Methods (7) 

      /// <summary>
      /// Creates a new <see cref="Org.Edgerunner.Dynamics.Nav.CSide.ApplicationEventSubscriber"/> and subscribes the Client instance to its events.
      /// </summary>
      private void ConnectApplicationEvents()
      {
         _Subscriber = new ApplicationEventSubscriber(this);
         _Subscriber.Advise(ref _objectDesigner);
      }

      /// <summary>
      /// Unsubscribes the current <see cref="Org.Edgerunner.Dynamics.Nav.CSide.ApplicationEventSubscriber"/>
      /// </summary>
      private void DisconnectApplicationEvents()
      {
         if (_Subscriber != null)
            _Subscriber.Unadvise();
         _Subscriber = null;
      }

      /// <summary>
      /// Posts the Activated event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostActivatedEvent(object state)
      {
         _Activated(this, state as CSideEventArgs);
      }

      /// <summary>
      /// Posts the company changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostCompanyChangedEvent(object state)
      {
         _CompanyChanged(this, state as CSideEventArgs);
      }

      /// <summary>
      /// Posts the database changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostDatabaseChangedEvent(object state)
      {
         _DatabaseChanged(this, state as CSideEventArgs);
      }

      /// <summary>
      /// Posts the Deactivated event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostDeactivatedEvent(object state)
      {
         _Deactivated(this, state as CSideEventArgs);
      }

      /// <summary>
      /// Posts the server changed event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void PostServerChangedEvent(object state)
      {
         _ServerChanged(this, state as CSideEventArgs);
      }

      // Internal Methods (8) 

      /// <summary>
      /// Raises the Activated event.
      /// </summary>
      /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
      internal void RaiseActivated(CSideEventArgs args)
      {
         if (_Activated != null)
         {
            if (_Context != null)
               _Context.Post(PostActivatedEvent, args);
            else
               PostActivatedEvent(args);
         }
      }

      /// <summary>
      /// Raises the CompanyChanged.
      /// </summary>
      /// <param name="args">The <see cref="Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs"/> instance containing the event data.</param>
      internal void RaiseCompanyChanged(CSideEventArgs args)
      {
         if (_CompanyChanged != null)
         {
            if (_Context != null)
               _Context.Post(PostCompanyChangedEvent, args);
            else
               PostCompanyChangedEvent(args);
         }
      }

      /// <summary>
      /// Raises the DatabaseChanged.
      /// </summary>
      /// <param name="args">The <see cref="Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs"/> instance containing the event data.</param>
      internal void RaiseDatabaseChanged(CSideEventArgs args)
      {
         if (_DatabaseChanged != null)
         {
            if (_Context != null)
               _Context.Post(PostDatabaseChangedEvent, args);
            else
               PostDatabaseChangedEvent(args);
         }
      }

      /// <summary>
      /// Raises the Deactivated event.
      /// </summary>
      /// <param name="args">The <see cref="System.EventArgs"/> instance containing the event data.</param>
      internal void RaiseDeactivated(CSideEventArgs args)
      {
         if (_Deactivated != null)
         {
            if (_Context != null)
               _Context.Post(PostDeactivatedEvent, args);
            else
               PostDeactivatedEvent(args);
         }
      }

      /// <summary>
      /// Raises the FormOpened event.
      /// </summary>
      /// <param name="form">The form.</param>
      internal void RaiseFormOpened(CSideEventArgs args)
      {
         if (_FormOpened != null)
            _FormOpened(this, args);
      }

      /// <summary>
      /// Raises the ServerChanged event.
      /// </summary>
      /// <param name="args">The <see cref="Org.Edgerunner.Dynamics.Nav.CSide.CSideEventArgs"/> instance containing the event data.</param>
      internal void RaiseServerChanged(CSideEventArgs args)
      {
         if (_ServerChanged != null)
         {
            if (_Context != null)
               _Context.Post(PostServerChangedEvent, args);
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
         IntPtr pcbWritten = new IntPtr((void*)&num);
         IStream pOutStm = null;
         CreateStreamOnHGlobal(0, true, out pOutStm);
         pOutStm.Write(buffer, buffer.Length, pcbWritten);
         pOutStm.Seek((long)0, 0, IntPtr.Zero);
         return pOutStm;
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

      #endregion Methods

      #region IObjectDesigner functionality

      /// <summary>
      /// Compiles the specified object.
      /// </summary>
      /// <param name="navObjectType">Type of the Nav object.</param>
      /// <param name="objectID">The object ID.</param>
      public void CompileObject(NavObjectType navObjectType, int objectID)
      {
         int result = _objectDesigner.CompileObject((int)navObjectType, objectID);
         if (result != 0)
            throw CSideException.GetException(result);
      }

      /// <summary>
      /// Compiles the objects within the supplied filter.
      /// </summary>
      /// <param name="filter">The filter.</param>
      public void CompileObjects(string filter)
      {
         int result = _objectDesigner.CompileObjects(filter);
         if (result != 0)
            throw CSideException.GetException(result);
      }

      /// <summary>
      /// Gets the company name.
      /// </summary>
      /// <value>The company.</value>
      public string Company
      {
         get
         {
            string companyName;
            _objectDesigner.GetCompanyName(out companyName);
            if (string.IsNullOrEmpty(companyName))
               return string.Empty;
            else
               return companyName;
         }
      }

      /// <summary>
      /// Gets the database name.
      /// </summary>
      /// <value>The database.</value>
      public string Database
      {
         get
         {
            string databaseName;
            _objectDesigner.GetDatabaseName(out databaseName);
            if (string.IsNullOrEmpty(databaseName))
               return string.Empty;
            else
               return databaseName;
         }
      }

      /// <summary>
      /// Gets the server name.
      /// </summary>
      /// <value>The server.</value>
      public string Server
      {
         get
         {
            string serverName;
            _objectDesigner.GetServerName(out serverName);
            if (string.IsNullOrEmpty(serverName))
               return string.Empty;
            else
               return serverName;
         }
      }

      /// <summary>
      /// Gets the type of the server.
      /// </summary>
      /// <value>The type of the server.</value>
      public ServerType ServerType
      {
         get
         {
            int serverType = 0;
            _objectDesigner.GetServerType(out serverType);
            return (ServerType)serverType;
         }
      }

      /// <summary>
      /// Gets the C/Side version.
      /// </summary>
      /// <value>The C/Side version.</value>
      public string CSideVersion
      {
         get
         {
            string csideVersion;
            int result = _objectDesigner.GetCSIDEVersion(out csideVersion);
            if (result != 0)
               throw CSideException.GetException(result);
            return csideVersion;
         }
      }

      /// <summary>
      /// Gets the application version.
      /// </summary>
      /// <value>The application version.</value>
      public string ApplicationVersion
      {
         get
         {
            string appVersion;
            int result = _objectDesigner.GetApplicationVersion(out appVersion);
            if (result != 0)
               throw CSideException.GetException(result);
            return appVersion;
         }
      }

      /// <summary>
      /// Reads the specified object to a stream.
      /// </summary>
      /// <param name="navObjectType">Type of the Navision object.</param>
      /// <param name="objectId">The object ID.</param>
      /// <returns>A <see cref="System.IO.MemoryStream"/> containing the text for the specified object</returns>
      public MemoryStream ReadObjectToStream(NavObjectType navObjectType, int objectID)
      {
         IStream pOutStm = null;
         CreateStreamOnHGlobal(0, true, out pOutStm);
         int result = _objectDesigner.ReadObject((int)navObjectType, objectID, pOutStm);
         if (result != 0)
            throw CSideException.GetException(result);
         return ToMemoryStream(pOutStm);
      }

      /// <summary>
      /// Writes the specified text object to Navision from a stream.
      /// </summary>
      /// <param name="stream">The stream containing the text for a Navision object.</param>
      public void WriteObjectFromStream(Stream stream)
      {
         IStream source = ToIStream(stream);
         int result = _objectDesigner.WriteObjects(source);
         if (result != 0)
            throw CSideException.GetException(result);
      }
      #endregion

      #region INSAppBase functionality

      /// <summary>
      /// Gets the specified table.
      /// </summary>
      /// <param name="tableNo">The table ID.</param>
      /// <returns>An instance of a <see cref="Org.Edgerunner.Dynamics.Nav.CSide.INSTable"/> or <c>null</c> if unable to get a reference</returns>
      internal INSTable GetTable(int tableID)
      {
         INSTable table;
         INSAppBase appBase = _objectDesigner as INSAppBase;
         if (appBase == null)
            return null;
         appBase.GetTable(tableID, out table);
         return table;
      }

      /// <summary>
      /// Enumerates the tables in the database the client is attached to.
      /// </summary>
      /// <param name="enumerator">The enumerator.</param>
      /// <param name="tableID">The table ID.</param>
      internal void EnumTables(CallbackEnumerator enumerator, int tableID)
      {
         INSAppBase appBase = _objectDesigner as INSAppBase;
         if (appBase == null)
            return;
         appBase.EnumTables(enumerator, tableID);
      }


      /// <summary>
      /// Retrieves an Table instance that corresponds to the table ID provided.
      /// </summary>
      /// <param name="tableID">The table ID.</param>
      /// <returns>A <see cref="Table"/> instance</returns>
      /// <remarks>Attempts to retrieve the table name via EnumTables(), but some table names cannot be obtained this way.
      /// If the name cannot be retrieved then it is set to "[Name Unavailable]".  This is usually only for some virtual tables.</remarks>
      public Table FetchTable(int tableID)
      {
         Table result;
         INSTable backingTable;
         INSAppBase appBase = _objectDesigner as INSAppBase;
         if (appBase == null)
            return null;
         CallbackEnumerator cbEnum = new CallbackEnumerator(this);
         EnumTables(cbEnum, tableID);
         Dictionary<Int32, Table> tables = cbEnum.Tables;
         if (tables.Count != 0)
            result = tables[tableID];
         else
         {
            result = new Table(tableID, "[Name Unavailable]", this);
            Int32 error = appBase.GetTable(tableID, out backingTable);
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
         INSAppBase appBase = _objectDesigner as INSAppBase;
         if (appBase == null)
            return;
         int result = appBase.StartTrans();
         if (result != 0)
            throw CSideException.GetException(result);
      }

      /// <summary>
      /// Triggers an error to be displayed inside the Navision client instance with the supplied message.
      /// </summary>
      /// <param name="message">The message to be displayed.</param>
      public void Error(string message)
      {
         INSAppBase appBase = _objectDesigner as INSAppBase;
         if (appBase == null)
            return;
         int result = appBase.Error(message);
         if (result != 0)
            throw CSideException.GetException(result);
      }

      /// <summary>
      /// The purpose of this method is currently unknown.  Hopefully this can be discovered and later documented
      /// </summary>
      /// <param name="flag">if set to <c>true</c> [flag].</param>
      /// <remarks>I added this method (which corresponds to proc6()) so that people could experiment with it</remarks>
      public void UnknownMethod(bool flag)
      {
         INSAppBase appBase = _objectDesigner as INSAppBase;
         if (appBase == null)
            return;
         appBase.proc6(flag);
      }

      /// <summary>
      /// Gets the tables in the database the client is attached.
      /// </summary>
      /// <value>The tables.</value>
      public Dictionary<Int32, Table> Tables
      {
         get
         {
            // I toyed with adding caching logic here to lower the overhead of repeated fetches but there were too many
            // problems with responding to table deletes/additions/changes.  Might add something later Due to this overhead.
            // It is recommended that you use FetchTable() to get a specific table.
            INSAppBase appBase = _objectDesigner as INSAppBase;
            if (appBase == null)
               return null;
            CallbackEnumerator cbEnum = new CallbackEnumerator(this);
            appBase.EnumTables(cbEnum, 0);
            return cbEnum.Tables;
         }
      }

      /// <summary>
      /// Gets the objects in the database the client is attached.
      /// </summary>
      /// <value>The objects.</value>
      public Dictionary<NavObjectType, Dictionary<int, Object>> Objects
      {
         get
         {
            if (_Objects == null)
            {
               // first we fetch the Object table
               Table table = FetchTable(2000000001);
               if (table == null)
                  throw new CSideException("Unable to retrieve the Object table");
               // Filter for blank company and objects of type other than tabledata
               table.SetFilter(1, ">0");
               table.SetFilter(2, "=''");
               List<Record> records = table.FetchRecords();
               _Objects = new Dictionary<NavObjectType, Dictionary<int, Object>>();
               foreach (NavObjectType objectType in Enum.GetValues(typeof(NavObjectType)))
                  _Objects.Add(objectType, new Dictionary<int, Object>());
               foreach (Record record in records)
               {
                  Object nObject = new Object(record);
                  _Objects[nObject.Type].Add(nObject.ID, nObject); ;
               }
            }
            return _Objects;
         }
      }
      #endregion

      #region INSHyperlink functionality

      /// <summary>
      /// Opens the hyperlink within the Navision client instance.
      /// </summary>
      /// <param name="link">The hyperlink.</param>
      /// <remarks>This hyperlink should be a navision hyperlink that corresponds to some object inside Navision</remarks>
      public void OpenLink(string link)
      {
         if (string.IsNullOrEmpty(link))
            return;
         INSHyperlink app = _objectDesigner as INSHyperlink;
         if (app != null)
            app.Open(link);
      }

      /// <summary>
      /// Gets the window handle for the Navision client instance.
      /// </summary>
      /// <value>The window handle.</value>
      public Int32 WindowHandle
      {
         get
         {
            INSHyperlink app = _objectDesigner as INSHyperlink;
            Int32 handle;
            if (app == null)
               return 0;
            app.GetNavWindowHandle(out handle);
            return handle;
         }
      }


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
            INSApplication app = _objectDesigner as INSApplication;
            if (app == null)
               return null;
            INSForm form;
            int result = app.GetCurrentForm(out form);
            if (result != 0)
               throw CSideException.GetException(result);
            return new Form(this, form);
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
         DisconnectApplicationEvents();
         if (_objectDesigner != null)
            Marshal.ReleaseComObject(_objectDesigner);
      }
      #endregion
   }
}

