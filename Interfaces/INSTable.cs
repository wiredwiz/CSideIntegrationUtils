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

using System.Runtime.InteropServices;

namespace Org.Edgerunner.Dynamics.Nav.CSide.Interfaces
{
   [ComImport, Guid("50000004-0000-1000-0006-0000836BD2D2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
   internal interface INSTable
   {
      /// <summary>
      /// Deletes the specified record.
      /// </summary>
      /// <param name="rec">The record to delete.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int Delete([In, MarshalAs(UnmanagedType.Interface)] INSRec rec);

      /// <summary>
      /// Inserts the specified record.
      /// </summary>
      /// <param name="rec">The record to insert.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int Insert([In, MarshalAs(UnmanagedType.Interface)] INSRec rec);

      /// <summary>
      /// Modifies the specified record.
      /// </summary>
      /// <param name="rec">The record to modify.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int Modify([In, MarshalAs(UnmanagedType.Interface)] INSRec rec);

      /// <summary>
      /// Initializes the specified record.
      /// </summary>
      /// <param name="rec">The record to initialize.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int Init([Out, MarshalAs(UnmanagedType.Interface)] out INSRec rec);

      /// <summary>
      /// Sets the filter for the specified field.
      /// </summary>
      /// <param name="fieldNo">The field number of the field to set the filter for.</param>
      /// <param name="filterValue">The filter value to set.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int SetFilter([In] int fieldNo, [In] string filterValue);

      /// <summary>
      /// Enumerates the filters for all fields in the table.
      /// </summary>
      /// <param name="callback">The callback enumerator instance to use.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int EnumFilters([In, MarshalAs(UnmanagedType.Interface)] INSCallbackEnum callback);

      /// <summary>
      /// Enumerates the records in the table.
      /// </summary>
      /// <param name="callback">The callback enumerator instance to use.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int EnumRecords([In, MarshalAs(UnmanagedType.Interface)] INSCallbackEnum callback);

      /// <summary>
      /// Enumerates the fields in the table.
      /// </summary>
      /// <param name="callback">The callback enumerator instance to use.</param>
      /// <param name="languageID">The language identifier to use.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      /// <remarks>The language id is used to fetch the appropriate field caption in the case of multi-language captions.</remarks>
      int EnumFields([In, MarshalAs(UnmanagedType.Interface)] INSCallbackEnum callback, [In] int languageID);

      /// <summary>
      /// Finds the specified record.
      /// </summary>
      /// <param name="rec">The record to fetch based on primary key values.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int Find([In, MarshalAs(UnmanagedType.Interface)] INSRec rec);

      /// <summary>
      /// Gets the table number that uniquely identifies this table.
      /// </summary>
      /// <param name="tableId">The table number.</param>
      /// <returns>A System.Int32 where 0 indicates success and a non-zero value indicates an error code.</returns>
      int GetID(out int tableId);

      /// <summary>
      /// The purpose of this method is not currently known.
      /// </summary>
      /// <param name="a">An integer value passed by reference for return.</param>
      /// <returns>A System.Int32 value.</returns>
      int Proc13(out int a);
   }
}
