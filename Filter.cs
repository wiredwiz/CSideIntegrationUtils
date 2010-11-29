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
   /// Represents the filter(s) for a field in a Dynamics Nav client instance
   /// </summary>
   public struct Filter
   {
		#region Data Members (3) 

      /// <summary>
      /// Gets or sets the FieldNo
      /// </summary>
      public readonly Int32 FieldNo;
      private readonly Table _Table;
      /// <summary>
      /// Gets or sets the Value.
      /// </summary>
      public readonly string Value;

		#endregion Data Members 

      #region Constructors/Deconstructors (1)
      /// <summary>
      /// Initializes a new instance of the Filter structure.
      /// </summary>
      /// <param name="fieldNo"></param>
      /// <param name="value"></param>
      /// <param name="record"></param>
      public Filter(Int32 fieldNo, string value, Table table)
      {
         FieldNo = fieldNo;
         Value = value;
         _Table = table;
      }
      #endregion

   }
}
