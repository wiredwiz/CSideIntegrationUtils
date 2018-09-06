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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Org.Edgerunner.Dynamics.Nav.CSide.Exceptions;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// Represents a form in a Dynamics Nav client instance.
   /// </summary>
   public class Form : IDisposable
   {
		#region Non-Public Fields (2) 

      private Client _Client;
      private INSForm _Form;

		#endregion Non-Public Fields 

		#region Properties (2) 

      /// <summary>
      /// Gets the ID.
      /// </summary>
      /// <value>The ID.</value>
      public int ID
      {
         get
         {
            int id;
            string idText;
            int result = _Form.GetID(out idText);
            Regex regex = new Regex(@"\d+",RegexOptions.CultureInvariant);
            Match match = regex.Match(idText);
            if (!match.Success)
               throw new Exception(String.Format("Unexpected form id format returned: {0}", idText));
            id = Int32.Parse(match.Value);
            return id;
         }
      }

      /// <summary>
      /// Gets the language ID.
      /// </summary>
      /// <value>The language ID.</value>
      public int LanguageID
      {
         get
         {
            int id;
            int result = _Form.GetLanguageID(out id);
            if (result != 0)
               throw CSideException.GetException(result);
            return id;
         }
      }

		#endregion Properties 

		#region Constructors/Deconstructors (2) 

      /// <summary>
      /// Initializes a new instance of the Form class.
      /// </summary>
      /// <param name="client"></param>
      /// <param name="form"></param>
      internal Form(Client client, INSForm form)
      {
         _Client = client;
         _Form = form;
      }

      /// <summary>
      /// Releases unmanaged resources and performs other cleanup operations before the
      /// <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Form"/> is reclaimed by garbage collection.
      /// </summary>
      ~Form()
      {
         Dispose(false);
      }

		#endregion Constructors/Deconstructors 

		#region Methods (3) 

		// Public Methods (3) 

      /// <summary>
      /// Gets the hyperlink that can be used to open this form directly.
      /// </summary>
      /// <returns>A <see cref="System.String"/> containing a hyperlink</returns>
      public string GetHyperlink()
      {
         string hyperlink;
         int result = _Form.GetHyperlink(out hyperlink);
         if (result != 0)
            throw CSideException.GetException(result);
         return hyperlink;
      }

      /// <summary>
      /// Gets the table that the form is based on.
      /// </summary>
      /// <returns>A <see cref="Org.Edgerunner.Dynamics.Nav.CSide.Table"/></returns>
      public Table GetTable()
      {
         // Fetch INSTable
         INSTable backingTable;
         Table table;
         int result = _Form.GetTable(out backingTable);
         if (result != 0)
            throw CSideException.GetException(result);
         // Now attempt to fetch the name of the table
         // first get the ID
         int tableID;
         result = backingTable.GetID(out tableID);
         if (result != 0)
            throw CSideException.GetException(result);
         // now enumerate by the ID to get the name
         CallbackEnumerator cbEnum = new CallbackEnumerator(_Client);
         _Client.EnumTables(cbEnum, tableID);
         if (cbEnum.Tables.Count != 0)
         {
            table = cbEnum.Tables[tableID];
            table.SetBackingTable(backingTable);
         }
         else
            table = new Table(ID, "[Name Unavailable]", _Client);
         return table;
      }

      /// <summary>
      /// Saves the current form record and updates the form to display the latest data.
      /// </summary>
      public void Update()
      {
         _Form.Update();
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
         if (_Form != null)
            Marshal.ReleaseComObject(_Form);
      }

      #endregion
   }
}
