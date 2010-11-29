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
using System.Collections.Generic;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// A CallbackEnumerator for enumerating tables, records, fields, field values, and filters in a Dynamics Nav client instance
   /// </summary>
   internal class CallbackEnumerator : INSCallbackEnum
   {
		#region Non-Public Fields (6) 

      private Dictionary<Int32, Field> _Fields;
      private Dictionary<Int32, FieldValue> _FieldValues;
      private Dictionary<Int32, Filter> _Filters;
      private object _Manager;
      private List<Record> _Records;
      private Dictionary<Int32, Table> _Tables;

		#endregion Non-Public Fields 

		#region Properties (5) 

      /// <summary>
      /// Gets the enumerated fields.
      /// </summary>
      /// <value>The fields.</value>
      public Dictionary<Int32, Field> Fields
      {
         get { return _Fields; }
      }

      /// <summary>
      /// Gets the enumerated field values.
      /// </summary>
      /// <value>The field values.</value>
      public Dictionary<Int32, FieldValue> FieldValues
      {
         get { return _FieldValues; }
      }

      /// <summary>
      /// Gets the enumerated filters.
      /// </summary>
      /// <value>The filters.</value>
      public Dictionary<Int32, Filter> Filters
      {
         get { return _Filters; }
      }

      /// <summary>
      /// Gets the enumerated records.
      /// </summary>
      /// <value>The records.</value>
      public List<Record> Records
      {
         get { return _Records; }
      }

      /// <summary>
      /// Gets the enumerated tables.
      /// </summary>
      /// <value>The tables.</value>
      public Dictionary<Int32, Table> Tables
      {
         get { return _Tables; }
      }

		#endregion Properties 

		#region Constructors/Deconstructors (1) 

      /// <summary>
      /// Initializes a new instance of the CallbackEnumerator class.
      /// </summary>
      /// <param name="manager"></param>
      public CallbackEnumerator(object manager)
      {
         _Manager = manager;
         _Fields = new Dictionary<int, Field>();
         _FieldValues = new Dictionary<int, FieldValue>();
         _Tables = new Dictionary<int, Table>();
         _Filters = new Dictionary<int, Filter>();
         _Records = new List<Record>();
      }

		#endregion Constructors/Deconstructors 

      #region INSCallbackEnum Members

      /// <summary>
      /// Receives the next record.
      /// </summary>
      /// <param name="record">The record.</param>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int NextRecord(INSRec record)
      {
         Record rec = new Record(record, _Manager as Table);
         CallbackEnumerator cbEnum = new CallbackEnumerator(rec);
         record.EnumFieldValues(cbEnum);
         rec._Fields = cbEnum.FieldValues;
         _Records.Add(rec);
         return 0;
      }

      /// <summary>
      /// Receives the next field value.
      /// </summary>
      /// <param name="fieldNo">The field no.</param>
      /// <param name="fieldValue">The field value.</param>
      /// <param name="dataType">Type of the data.</param>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int NextFieldValue(int fieldNo, string fieldValue, string dataType)
      {
         _FieldValues.Add(fieldNo, new FieldValue(fieldNo, fieldValue, dataType, _Manager as Record));
         return 0;
      }

      /// <summary>
      /// Receives the next filter value.
      /// </summary>
      /// <param name="fieldNo">The field no.</param>
      /// <param name="filterValue">The filter value.</param>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int NextFilterValue(int fieldNo, string filterValue)
      {
         _Filters.Add(fieldNo, new Filter(fieldNo, filterValue, _Manager as Table));
         return 0;
      }

      /// <summary>
      /// Receives information about the next table.
      /// </summary>
      /// <param name="tableID">The table ID.</param>
      /// <param name="tableName">Name of the table.</param>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int NextTable(int tableID, string tableName)
      {
         _Tables.Add(tableID, new Table(tableID, tableName, _Manager as Client));
         return 0;
      }

      /// <summary>
      /// Receives the next field definition.
      /// </summary>
      /// <param name="fieldNo">The field no.</param>
      /// <param name="fieldName">Name of the field.</param>
      /// <param name="fieldCaption">The field caption.</param>
      /// <param name="dataType">Type of the data.</param>
      /// <param name="dataLength">Length of the data.</param>
      /// <param name="flag">Unknown flag.</param>
      /// <returns>An <see cref="System.Int32"/> representing an error code</returns>
      public int NextFieldDef(int fieldNo, string fieldName, string fieldCaption, string dataType, int dataLength, int flag)
      {
         _Fields.Add(fieldNo, new Field(_Manager as Table, fieldNo, fieldName, fieldCaption, dataType, dataLength, flag));
         return 0;
      }

      #endregion
   }
}
