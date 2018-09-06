#region Apache License 2.0
// <copyright company="Edgerunner.org" file="DatabaseChangedEventArgs.cs">
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

namespace Org.Edgerunner.Dynamics.Nav.CSide.EventArguments
{
   /// <summary>
   /// Class that represents event argument information when the database changes.
   /// </summary>
   /// <seealso cref="Org.Edgerunner.Dynamics.Nav.CSide.EventArguments.NameChangeEventArgs" />
   public class DatabaseChangedEventArgs : NameChangeEventArgs
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="DatabaseChangedEventArgs"/> class.
      /// </summary>
      /// <param name="previousDatabase">The previous database name.</param>
      /// <param name="newDatabase">The new database name.</param>
      public DatabaseChangedEventArgs(string previousDatabase, string newDatabase)
         : base(previousDatabase, newDatabase)
      {
      }
   }
}