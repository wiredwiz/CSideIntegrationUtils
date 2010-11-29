//
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

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents an object within Microsoft Dynamics Nav.
   /// </summary>
   public class Object : IDisposable
   {
		#region Non-Public Fields (1) 

      private Record _Record;

		#endregion Non-Public Fields 

      /// <summary>
      /// Refreshes this object instance.
      /// </summary>
      /// <remarks>This should always be called before accessing any properties if you must have the most up to date information</remarks>
      public void Refresh()
      {
         _Record.Refresh();
      }

		#region Properties (9) 

      /// <summary>
      /// Gets the Blob Size of the object.
      /// </summary>
      /// <value>The size of the BLOB.</value>
      public int BlobSize
      {
         get
         {
            return int.Parse(_Record.FieldValues[8].Value);
         }
      }

      /// <summary>
      /// Gets a value indicating whether this <see cref="Object"/> is compiled.
      /// </summary>
      /// <value><c>true</c> if compiled; otherwise, <c>false</c>.</value>
      public bool Compiled
      {
         get
         {
            return Int32.Parse(_Record.FieldValues[6].Value) == 1;
         }
      }

      /// <summary>
      /// Gets the ID.
      /// </summary>
      /// <value>The ID.</value>
      public int ID
      {
         get
         {
            return int.Parse(_Record.FieldValues[3].Value);
         }
      }

      /// <summary>
      /// Gets a value indicating whether this <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Object"/> is modified.
      /// </summary>
      /// <value><c>true</c> if modified; otherwise, <c>false</c>.</value>
      public bool Modified
      {
         get
         {
            return Int32.Parse(_Record.FieldValues[5].Value) == 1;
         }
      }

      /// <summary>
      /// Gets the name.
      /// </summary>
      /// <value>The name.</value>
      public string Name
      {
         get
         {
            return _Record.FieldValues[4].Value;
         }
      }


      /// <summary>
      /// Gets the last modified time.
      /// </summary>
      /// <value>The last modified time.</value>
      public DateTime LastModifiedTime
      {
         get
         {
            DateTime lmDate = DateTime.Parse(_Record.FieldValues[10].Value);
            DateTime lmTime = DateTime.Parse(_Record.FieldValues[11].Value);
            return new DateTime(lmDate.Year, lmDate.Month, lmDate.Day, lmTime.Hour, lmTime.Minute, lmTime.Second);
         }
      }

      /// <summary>
      /// Gets the type.
      /// </summary>
      /// <value>The type.</value>
      public NavObjectType Type
      {
         get
         {
            return (NavObjectType)int.Parse(_Record.FieldValues[1].Value);
         }
      }

      /// <summary>
      /// Gets the version list.
      /// </summary>
      /// <value>The version list.</value>
      public string VersionList
      {
         get
         {
            return _Record.FieldValues[12].Value;
         }
      }

		#endregion Properties 

		#region Constructors/Deconstructors (1) 

      /// <summary>
      /// Initializes a new instance of the Object class.
      /// </summary>
      /// <param name="record"></param>
      internal Object(Record record)
      {
         _Record = record;
      }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         _Record.Dispose();
         GC.SuppressFinalize(this);
      }


      #endregion
   }
}
