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

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// An event arguments class for C/Side events
   /// </summary>
   public class CSideEventArgs : EventArgs
   {
		#region Properties (10) 

      /// <summary>
      /// Gets or sets the current company.
      /// </summary>
      /// <value>The current company.</value>
      public string CurrentCompany { get; set; }

      /// <summary>
      /// Gets or sets the current database.
      /// </summary>
      /// <value>The current database.</value>
      public string CurrentDatabase { get; set; }

      /// <summary>
      /// Gets or sets the current server.
      /// </summary>
      /// <value>The current server.</value>
      public string CurrentServer { get; set; }

      /// <summary>
      /// Gets or sets the type of the current server.
      /// </summary>
      /// <value>The type of the current server.</value>
      public ServerType CurrentServerType { get; set; }

      /// <summary>
      /// Gets or sets the form.
      /// </summary>
      /// <value>The form.</value>
      public Form Form { get; set; }

      /// <summary>
      /// Gets or sets the previous company.
      /// </summary>
      /// <value>The previous company.</value>
      public string PreviousCompany { get; set; }

      /// <summary>
      /// Gets or sets the previous database.
      /// </summary>
      /// <value>The previous database.</value>
      public string PreviousDatabase { get; set; }

      /// <summary>
      /// Gets or sets the previous server.
      /// </summary>
      /// <value>The previous server.</value>
      public string PreviousServer { get; set; }

      /// <summary>
      /// Gets or sets the type of the previous server.
      /// </summary>
      /// <value>The type of the previous server.</value>
      public ServerType PreviousServerType { get; set; }

      /// <summary>
      /// Argument used by unknown event
      /// </summary>
      public string UnknownArg { get; set; }

		#endregion Properties 

		#region Constructors/Deconstructors (4) 

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs(ServerType previousServerType, string previousServer, ServerType currentServerType, string currentServer)
      {
         CurrentServer = currentServer;
         CurrentServerType = currentServerType;
         PreviousServer = previousServer;
         PreviousServerType = previousServerType;
      }

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs(ChangeEventType type, string previous, string current)
      {
         switch (type)
         {
            case ChangeEventType.Database:
               PreviousDatabase = previous;
               CurrentDatabase = current;
               break;
            case ChangeEventType.Company:
               PreviousCompany = previous;
               CurrentCompany = current;
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs(Form form, string unknown)
         : this(form)
      {
         UnknownArg = unknown;
      }

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs(Form form)
      {
         Form = form;
      }

      /// <summary>
      /// Initializes a new instance of the CSideEventArgs class.
      /// </summary>
      public CSideEventArgs()
      {

      }

		#endregion Constructors/Deconstructors 
   }
}
