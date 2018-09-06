#region Apache License 2.0
// <copyright company="Edgerunner.org" file="ServerChangedEventArgs.cs">
// Copyright (c) Thaddeus Ryker 2018
// </copyright>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;

namespace Org.Edgerunner.Dynamics.Nav.CSide.EventArguments
{
   /// <summary>
   /// Class that represents event argument information when the server changes.
   /// </summary>
   public class ServerChangedEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ServerChangedEventArgs"/> class.
      /// </summary>
      /// <param name="previousServerType">Type of the previous server.</param>
      /// <param name="previousServerName">Name of the previous server.</param>
      /// <param name="newServerType">New type of the server.</param>
      /// <param name="newServerName">New name of the server.</param>
      public ServerChangedEventArgs(ServerType previousServerType, string previousServerName, ServerType newServerType, string newServerName)
      {
         PreviousServerType = previousServerType;
         NewServerType = newServerType;
         PreviousServerName = previousServerName;
         NewServerName = newServerName;
      }

      /// <summary>
      /// Gets or sets the previous server type.
      /// </summary>
      /// <value>The previous server type.</value>
      public ServerType PreviousServerType { get; set; }

      /// <summary>
      /// Gets or sets the new server type.
      /// </summary>
      /// <value>The new server type.</value>
      public ServerType NewServerType { get; set; }

      /// <summary>
      /// Gets or sets the name of the previous server.
      /// </summary>
      /// <value>The name of the previous server.</value>
      public string PreviousServerName { get; set; }

      /// <summary>
      /// Gets or sets the name of the new server.
      /// </summary>
      /// <value>The name of the new server.</value>
      public string NewServerName { get; set; }
   }
}