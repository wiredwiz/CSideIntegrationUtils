﻿#region Apache License 2.0
// <copyright company="Edgerunner.org" file="NameChangeEventArgs.cs">
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
   public abstract class NameChangeEventArgs : EventArgs
   {
      protected NameChangeEventArgs(string previousName, string newName)
      {
         Previous = previousName;
         New = newName;
      }

      /// <summary>
      /// Gets or sets the previous name.
      /// </summary>
      /// <value>The previous name.</value>
      public string Previous { get; set; }

      /// <summary>
      /// Gets or sets the new name.
      /// </summary>
      /// <value>The new name.</value>
      public string New { get; set; }
   }
}