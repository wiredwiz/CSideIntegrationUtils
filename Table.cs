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
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents a table in a Dynamics Nav client instance.
   /// </summary>
   public class Table : IDisposable
   {
		#region Non-Public Fields (4) 

      private string _Name;
      private Int32 _TableID;
      internal Client _Client;
      internal INSTable _Table;

		#endregion Non-Public Fields 

		#region Properties (4) 

      /// <summary>
      /// Gets and sets the culture to use for the table
      /// </summary>
      public CultureInfo Culture { get; set; }

      /// <summary>
      /// Gets the fields.
      /// </summary>
      /// <value>The fields.</value>
      public Dictionary<Int32, Field> Fields
      {
         get
         {
            FetchBackingTableIfNeeded();
            CallbackEnumerator cbEnum = new CallbackEnumerator(this);
            _Table.EnumFields(cbEnum, Culture.LCID);
            return cbEnum.Fields;
         }
      }

      /// <summary>
      /// Gets the filters.
      /// </summary>
      /// <value>The filters.</value>
      public Dictionary<Int32, Filter> Filters
      {
         get
         {
            FetchBackingTableIfNeeded();
            CallbackEnumerator cbEnum = new CallbackEnumerator(this);
            _Table.EnumFilters(cbEnum);
            return cbEnum.Filters;
         }
      }

      /// <summary>
      /// Gets the name.
      /// </summary>
      /// <value>The name.</value>
      public string Name
      {
         get { return _Name; }
      }

      /// <summary>
      /// Gets the table ID.
      /// </summary>
      /// <value>The table ID.</value>
      public Int32 TableID
      {
         get { return _TableID; }
      }

		#endregion Properties 

		#region Constructors/Deconstructors (2) 

      /// <summary>
      /// Initializes a new instance of the Table class.
      /// </summary>
      /// <param name="tableNo"></param>
      /// <param name="name"></param>
      /// <param name="client"></param>
      public Table(Int32 tableNo, string name, Client client)
      {
         Culture = CultureInfo.CurrentCulture;
         _Name = name;
         _Client = client;
         _TableID = tableNo;
      }

      /// <summary>
      /// Releases unmanaged resources and performs other cleanup operations before the
      /// <see cref="Table"/> is reclaimed by garbage collection.
      /// </summary>
      ~Table()
      {
         Dispose(false);
      }

		#endregion Constructors/Deconstructors 

		#region Methods (7) 

		// Private Methods (1) 

      /// <summary>
      /// Fetches the backing table if needed.
      /// </summary>
      private void FetchBackingTableIfNeeded()
      {
         if (_Table == null)
            _Table = _Client.GetTableInternal(TableID);
      }
		// Internal Methods (1) 

      /// <summary>
      /// Sets the backing table.
      /// </summary>
      /// <param name="table">The table.</param>
      internal void SetBackingTable(INSTable table)
      {
         _Table = table;
      }
		// Public Methods (5) 

      /// <summary>
      /// Deletes the record.
      /// </summary>
      /// <param name="record">The record.</param>
      public void DeleteRecord(Record record)
      {
         FetchBackingTableIfNeeded();
         _Table.Delete(record._Record);
      }

      /// <summary>
      /// Initializes the record.
      /// </summary>
      /// <param name="record">The record.</param>
      public void InitRecord(Record record)
      {
         FetchBackingTableIfNeeded();
         _Table.Init(out record._Record);
      }

      /// <summary>
      /// Inserts the record.
      /// </summary>
      /// <param name="record">The record.</param>
      public void InsertRecord(Record record)
      {
         FetchBackingTableIfNeeded();
         _Table.Insert(record._Record);
      }

      /// <summary>
      /// Modifies the record.
      /// </summary>
      /// <param name="record">The record.</param>
      public void ModifyRecord(Record record)
      {
         FetchBackingTableIfNeeded();
         _Table.Modify(record._Record);
      }

      /// <summary>
      /// Sets the filter.
      /// </summary>
      /// <param name="fieldNo">The field no.</param>
      /// <param name="filterValue">The filter value.</param>
      public void SetFilter(Int32 fieldNo, string filterValue)
      {
         FetchBackingTableIfNeeded();
         int result = _Table.SetFilter(fieldNo, filterValue);
         if (result != 0)
            throw CSideException.GetException(result);
      }

      /// <summary>
      /// Finds the specified record.
      /// </summary>
      /// <param name="record">The record.</param>
      public void Find(Record record)
      {
         int result = _Table.Find(record._Record);
         if (result != 0)
            throw CSideException.GetException(result);
      }

      /// <summary>
      /// Fetches the records in this instance.
      /// </summary>
      /// <returns></returns>
      public List<Record> FetchRecords()
      {
         FetchBackingTableIfNeeded();
         CallbackEnumerator cbEnum = new CallbackEnumerator(this);
         _Table.EnumRecords(cbEnum);
         return cbEnum.Records;
      }

      /// <summary>
      /// The purpose of this method is unknown.  Hopefully someone will discover and document it.
      /// </summary>
      /// <returns>Unknown</returns>
      public int Proc13()
      {
         lock (_Client.GetSyncObject())
         {
            int value;
            int result = _Table.proc13(out value);
            if (result != 0)
               throw CSideException.GetException(result);
            return value;
         }
      }

		#endregion Methods 

      #region IDisposable Members

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Releases unmanaged and - optionally - managed resources
      /// </summary>
      /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            // no managed resources to free
         }
         // free unmanaged resources
         if (_Table != null)
            Marshal.ReleaseComObject(_Table);
      }

      #endregion
   }
}
