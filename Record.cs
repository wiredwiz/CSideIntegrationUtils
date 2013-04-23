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
using System.Runtime.InteropServices;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents a Record from a table in a Dynamics Nav client instance.
   /// </summary>
   public class Record : IDisposable
   {
      #region Non-Public Fields (3)

      internal Dictionary<Int32, FieldValue> _Fields;
      private Table _Table;
      internal INSRec _Record;

      #endregion Non-Public Fields

      #region Properties (1)

      /// <summary>
      /// Gets the field values.
      /// </summary>
      /// <value>The field values.</value>
      public Dictionary<Int32, FieldValue> FieldValues
      {
         get
         {
            return _Fields;
         }
      }

      #endregion Properties

      #region Constructors/Deconstructors (3)

      /// <summary>
      /// Initializes a new instance of the Record class.
      /// </summary>
      /// <param name="record"></param>
      /// <param name="table"></param>
      internal Record(INSRec record, Table table)
      {
         _Record = record;
         _Table = table;
      }

      /// <summary>
      /// Initializes a new instance of the Record class.
      /// </summary>
      /// <param name="table"></param>
      public Record(Table table)
      {
         _Table = table;
      }

      /// <summary>
      /// Releases unmanaged resources and performs other cleanup operations before the
      /// <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Record"/> is reclaimed by garbage collection.
      /// </summary>
      ~Record()
      {
         Dispose(false);
      }

      #endregion Constructors/Deconstructors

      #region Methods (6)

      // Public Methods (6) 

      /// <summary>
      /// Rehydrates the record with fresh and valid underlying INSRec instance.
      /// </summary>
      private void RehydrateRecord()
      {
         _Table.InitRecord(this);
         foreach (FieldValue field in _Fields.Values)
            _Record.SetFieldValue(field.FieldNo, field._Value, false);
         _Table.Find(this);
      }

      /// <summary>
      /// Loads the matching INSRec for this record as needed.
      /// </summary>
      internal void LazyLoadBackingRecord()
      {
         if (_Record == null)
            RehydrateRecord();
      }

      /// <summary>
      /// Deletes this instance.
      /// </summary>
      public void Delete()
      {
         LazyLoadBackingRecord();
         _Table.DeleteRecord(this);
      }

      /// <summary>
      /// Gets the field value.
      /// </summary>
      /// <param name="fieldNo">The field no.</param>
      /// <returns></returns>
      internal string GetFieldValue(int fieldNo)
      {
         string fieldValue;
         int result = _Record.GetFieldValue(fieldNo, out fieldValue);
         if (result != 0)
            throw CSideException.GetException(result);
         return fieldValue;
      }

      /// <summary>
      /// Initializes this instance.
      /// </summary>
      public void Init()
      {
         _Table.InitRecord(this);
      }

      /// <summary>
      /// Inserts this instance.
      /// </summary>
      public void Insert()
      {
         LazyLoadBackingRecord();
         _Table.InsertRecord(this);
      }

      /// <summary>
      /// Modifies this instance.
      /// </summary>
      public void Modify()
      {
         LazyLoadBackingRecord();
         _Table.ModifyRecord(this);
      }

      /// <summary>
      /// Sets the field value.
      /// </summary>
      /// <param name="fieldNo">The field no.</param>
      /// <param name="value">The value.</param>
      /// <param name="validate">if set to <c>true</c> [validate].</param>
      internal void SetFieldValue(int fieldNo, string value, bool validate)
      {
         int result = _Record.SetFieldValue(fieldNo, value, validate);
         if (result != 0)
            throw CSideException.GetException(result);
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
         if (_Record != null)
            Marshal.ReleaseComObject(_Record);
      }

      #endregion
   }
}
