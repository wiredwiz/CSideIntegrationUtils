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

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents a table field in a Dynamics Nav client instance
   /// </summary>
   public struct Field
   {
		#region Data Members (13) 

      private string _Caption;
      private int _ID;
      private int _Length;
      private string _Name;
      private Table _Table;
      private string _Type;
      private int _Unknown;

      public string Caption { get { return _Caption; } }

      public int ID { get { return _ID; } }

      public int Length { get { return _Length; } }

      public string Name { get { return _Name; } }

      public string Type { get { return _Type; } }

      public int Unknown { get { return _Unknown; } }

		#endregion Data Members 

		#region Methods (1) 

      /// <summary>
      /// Initializes a new instance of the Field structure.
      /// </summary>
      /// <param name="table"></param>
      /// <param name="iD"></param>
      /// <param name="name"></param>
      /// <param name="caption"></param>
      /// <param name="type"></param>
      /// <param name="length"></param>
      /// <param name="unknown"></param>
      public Field(Table table, int iD, string name, string caption, string type, int length, int unknown)
      {
         _Table = table;
         _ID = iD;
         _Name = name;
         _Caption = caption;
         _Type = type;
         _Length = length;
         _Unknown = unknown;
      }

		#endregion Methods 
   }
}
