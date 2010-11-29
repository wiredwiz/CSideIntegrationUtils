﻿//
//Copyright 2010 Thaddeus L Ryker
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
using System.Threading;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// A class for subscribing to events from a running Navision client instance
   /// </summary>
   internal class ApplicationEventSubscriber : INSApplicationEvents
   {
      #region Non-Public Fields (6)

      private Client _Client;
      private UCOMIConnectionPoint _ConnectionPoint;
      private int _Cookie;
      private Guid _IID = new Guid("50000004-0000-1000-0004-0000836BD2D2");
      private int _ReferenceCount;

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

      #region Methods (3)

      // Public Methods (2) 

      /// <summary>
      /// Begins advising the linked Client of new events from the source.
      /// </summary>
      /// <param name="source">The source.</param>
      public void Advise(ref IObjectDesigner source)
      {
         //   Late bind connection point subscription
         if (++_ReferenceCount == 1)
         {
            //   Subscribe for connection point event
            UCOMIConnectionPointContainer ConnectionPointContainer = source as UCOMIConnectionPointContainer;
            ConnectionPointContainer.FindConnectionPoint(ref _IID, out _ConnectionPoint);
            _ConnectionPoint.Advise(this, out _Cookie);
         }
      }

      /// <summary>
      /// Stops advising the linked Client of new events.
      /// </summary>
      public void Unadvise()
      {
         //   Unsubscribe for connection point event upon last unadvise
         if (--_ReferenceCount == 0)
         {
            _ConnectionPoint.Unadvise(_Cookie);

            //   Decrement RCW count and set reference to null
            Marshal.ReleaseComObject(_ConnectionPoint);
            _Cookie = 0;
            _ConnectionPoint = null;
         }
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
      /// A stub to raise the FormOpen event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void RaiseFormOpenEvent(object state)
      {
         _Client.RaiseFormOpened(state as CSideEventArgs);
      }

      /// <summary>
      /// The purpose of this event is as yet unknown.  Hopefully this can be discovered and documented later.
      /// </summary>
      /// <param name="form">The form.</param>
      /// <param name="b">The b.</param>
      /// <returns></returns>
      public int proc4(INSForm form, string b)
      {
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
      /// A stub to raise the Activated event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void RaiseActivateEvent(object state)
      {
         _Client.RaiseActivated(state as CSideEventArgs);
      }

      /// <summary>
      /// RA stub to raise the Deactivated event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void RaiseDeactivateEvent(object state)
      {
         _Client.RaiseDeactivated(state as CSideEventArgs);
      }

      /// <summary>
      /// Called when the database and/or company of the subscribed Navision client changes.
      /// </summary>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int OnCompanyDBChange()
      {
         string previousDatabase = _Client.PreviousDatabase;
         string previousCompany = _Client.PreviousCompany;
         string currentDatabase = _Client.Database;
         string currentCompany = _Client.Company;
         _Client.PreviousDatabase = currentDatabase;
         _Client.PreviousCompany = currentCompany;
         if (previousDatabase != currentDatabase)
            ThreadPool.QueueUserWorkItem(RaiseDatabaseChangeEvent, new CSideEventArgs(ChangeEventType.Database, previousDatabase, currentDatabase));
         if (previousCompany != currentCompany)
            ThreadPool.QueueUserWorkItem(RaiseCompanyChangeEvent, new CSideEventArgs(ChangeEventType.Company, previousCompany, currentCompany));
         return 0;
      }

      /// <summary>
      /// Raises the CompanyChanged event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void RaiseCompanyChangeEvent(object state)
      {
         _Client.RaiseCompanyChanged(state as CSideEventArgs);
      }

      /// <summary>
      /// Raises the DatabaseChanged event.
      /// </summary>
      /// <param name="state">The state.</param>
      private void RaiseDatabaseChangeEvent(object state)
      {
         _Client.RaiseDatabaseChanged(state as CSideEventArgs);
      }

      #endregion
   }
}
