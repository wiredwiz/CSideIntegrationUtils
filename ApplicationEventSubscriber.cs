﻿//
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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;

using Org.Edgerunner.Dynamics.Nav.CSide.EventArguments;
using Org.Edgerunner.Dynamics.Nav.CSide.Interfaces;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// A class for subscribing to events from a running Navision client instance
   /// </summary>
   internal class ApplicationEventSubscriber : INSApplicationEvents
   {
      #region Non-Public Fields (5)

      private Client _Client;
      private IConnectionPoint _ConnectionPoint;
      private int _Cookie;
      private Guid _IID = new Guid("50000004-0000-1000-0004-0000836BD2D2");

      #endregion Non-Public Fields

      #region Constructors/Deconstructors (1)

      /// <summary>
      /// Initializes a new instance of the ApplicationEventSubscriber class.
      /// </summary>
      /// <param name="client"></param>
      public ApplicationEventSubscriber(Client client)
      {
         _Client = client;
      }

      #endregion Constructors/Deconstructors

      #region Methods (8)

      // Private Methods (6) 

      /// <summary>
      /// A stub to raise the Activated event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseActivateEvent(object state)
      {
         //_Client.RaiseActivated(state as CSideEventArgs);
      }

      /// <summary>
      /// A stub to raise the CompanyChanged event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseCompanyChangeEvent(object state)
      {
         _Client.RaiseCompanyChanged(state as CompanyChangedEventArgs);
      }

      /// <summary>
      /// A stub to raise the DatabaseChanged event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseDatabaseChangeEvent(object state)
      {
         _Client.RaiseDatabaseChanged(state as DatabaseChangedEventArgs);
      }

      /// <summary>
      /// A stub to raise the Deactivated event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseDeactivateEvent(object state)
      {
         //_Client.RaiseDeactivated(state as CSideEventArgs);
      }

      /// <summary>
      /// A stub to raise the FormOpened event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseFormOpenEvent(object state)
      {
         //_Client.RaiseFormOpened(state as CSideEventArgs);
      }

      /// <summary>
      /// A stub to raise the unknown event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseButtonClickEvent(object state)
      {
         //_Client.RaiseButtonClick(state as CSideEventArgs);
      }

      /// <summary>
      /// A stub to raise the ServerChanged event on a worker thread.
      /// </summary>
      /// <param name="state">The state.</param>
      /// <remarks>Calling this stub from a worker thread prevents the event from blocking the client where the event originated</remarks>
      private void RaiseServerChangeEvent(object state)
      {
         _Client.RaiseServerChanged(state as ServerChangedEventArgs);
      }
      // Public Methods (2) 

      /// <summary>
      /// Begins advising the linked <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> of new events from the source.
      /// </summary>
      /// <param name="source">The source.</param>
      public void Advise(ref IObjectDesigner source)
      {
         //   Late bind connection point subscription
         //   Subscribe for connection point event
         IConnectionPointContainer container = source as IConnectionPointContainer;
         container.FindConnectionPoint(ref _IID, out _ConnectionPoint);
         _ConnectionPoint.Advise(this, out _Cookie);
      }

      /// <summary>
      /// Stops advising the linked <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Client"/> of new events.
      /// </summary>
      public void Unadvise()
      {
         // Based on msdn, perhaps only one of the two call is necessary and not both.  It sounds like Unadvise triggers a call to Release
         // on the com object potentially making ReleaseComObject redundant

         //   Unsubscribe for connection point event upon last unadvise
         _ConnectionPoint.Unadvise(_Cookie);
         _Cookie = -1;

         // Decrement RCW count and set reference to null
         //Marshal.ReleaseComObject(_ConnectionPoint);
         _ConnectionPoint = null;
      }

      #endregion Methods

      #region INSApplicationEvents Members

      /// <summary>
      /// Called when a new form is opened within the subscribed Navision client.
      /// </summary>
      /// <param name="form">The form.</param>
      /// <returns></returns>
      public int OnFormOpen(INSForm form)
      {
         ThreadPool.QueueUserWorkItem(RaiseFormOpenEvent, new CSideEventArgs(new Form(_Client, form)));
         return 0;
      }

      /// <summary>
      /// Triggers when a button is clicked
      /// </summary>
      /// <param name="form">The form.</param>
      /// <param name="buttonKey">The button key.</param>
      /// <returns></returns>
      public int OnButtonClick(INSForm form, string buttonKey)
      {
         CSideEventArgs args = new CSideEventArgs(new Form(_Client, form), buttonKey);
         ThreadPool.QueueUserWorkItem(RaiseButtonClickEvent, args);
         return 0;
      }

      /// <summary>
      /// Called when the active status of the subscribed Navision client changes.
      /// </summary>
      /// <param name="active">if set to <c>true</c> [active].</param>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int OnActiveChanged(bool active)
      {
         CSideEventArgs args = new CSideEventArgs();
         if (active)
            ThreadPool.QueueUserWorkItem(RaiseActivateEvent, args);
         else
            ThreadPool.QueueUserWorkItem(RaiseDeactivateEvent, args);
         return 0;
      }

      /// <summary>
      /// Called when the database, server and/or company of the subscribed Navision client changes.
      /// </summary>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int OnCompanyDBChange()
      {
         // obtain previous settings
         string previousDatabase = _Client._PreviousDatabase;
         string previousCompany = _Client._PreviousCompany;
         ServerType previousServerType = _Client._PreviousServerType;
         string previousServer = _Client._PreviousServer;
         string currentDatabase = _Client.Database;
         string currentCompany = _Client.Company;
         ServerType currentServerType = _Client.ServerType;
         string currentServer = _Client.Server;
         // set previous setting variables to the new values
         _Client._PreviousDatabase = currentDatabase;
         _Client._PreviousCompany = currentCompany;
         _Client._PreviousServerType = currentServerType;
         _Client._PreviousServer = currentServer;
         // determine if the previous values are different from any of our newer ones and trigger appropriate events
         if ((previousServer != currentServer) || (previousServerType != currentServerType))
            ThreadPool.QueueUserWorkItem(RaiseServerChangeEvent, new CSideEventArgs(previousServerType, previousServer, currentServerType, currentServer));
         if (previousDatabase != currentDatabase)
            ThreadPool.QueueUserWorkItem(RaiseDatabaseChangeEvent, new CSideEventArgs(ChangeEventType.Database, previousDatabase, currentDatabase));
         if (previousCompany != currentCompany)
            ThreadPool.QueueUserWorkItem(RaiseCompanyChangeEvent, new CSideEventArgs(ChangeEventType.Company, previousCompany, currentCompany));
         return 0;
      }

      #endregion
   }
}
