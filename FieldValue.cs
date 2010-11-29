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
   /// Represents the value and data type of a field in a Dynamics Nav client instance
   /// </summary>
   public struct FieldValue
   {
		#region Data Members (4) 

      private readonly Record _Record;
      /// <summary>
      /// Gets or sets the DataType of the field.
      /// </summary>
      public readonly string DataType;
      /// <summary>
      /// Gets or sets the FieldNo of the field.
      /// </summary>
      public readonly Int32 FieldNo;
      private string _Value;
      /// <summary>
      /// Gets or sets the value of the field.
      /// </summary>
      /// <value>The value.</value>
      public string Value
      {
         get { return _Value; }
         set
         {
            SetValue(value, false);
         }
      }

		#endregion Data Members 

      #region Constructors/Deconstructors (1)

      /// <summary>
      /// Initializes a new instance of the Field structure.
      /// </summary>
      /// <param name="fieldNo"></param>
      /// <param name="value"></param>
      /// <param name="dataType"></param>
      /// <param name="record"></param>
      public FieldValue(Int32 fieldNo, string value, string dataType, Record record)
      {
         FieldNo = fieldNo;
         _Value = value;
         DataType = dataType;
         _Record = record;
      }
      #endregion

		#region Methods (10) 

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <param name="validate">if set to <c>true</c> [validate].</param>
      private void SetValue(string value, bool validate)
      {
         _Value = value;
         _Record.SetFieldValue(FieldNo, value, validate);
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <param name="validate">if set to <c>true</c> [validate].</param>
      public void SetValue(DateTime value, bool validate)
      {
         switch (DataType)
         {
            case "Date":
               SetValue(value.ToShortDateString(), validate);
               break;
            case "Time:":
               SetValue(value.ToShortTimeString(), validate);
               break;
            case "DateTime":
               SetValue(value.ToString(), validate);
               break;
            default:
               throw new CSideException(string.Format("Field no. {0} is not is not of type Date, Time, or DateTime", FieldNo));
               break;
         }
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <param name="validate">if set to <c>true</c> [validate].</param>
      public void SetValue(int value, bool validate)
      {
         switch (DataType)
         {
            case "Integer":
            case "Option":
               SetValue(value.ToString(), validate);
               break;
            default:
               throw new CSideException(string.Format("Field no. {0} is not of type Integer or Option", FieldNo));
         }        
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <param name="validate">if set to <c>true</c> [validate].</param>
      public void SetValue(decimal value, bool validate)
      {
         if (DataType != "Decimal")
            throw new CSideException(string.Format("Field no. {0} is not of type Decimal", FieldNo));
         SetValue(value.ToString(), validate);
      }

      /// <summary>
      /// Sets the value.
      /// </summary>
      /// <param name="value">the value.</param>
      /// <param name="validate">if set to <c>true</c> [validate].</param>
      public void SetValue(bool value, bool validate)
      {
         if (DataType != "Boolean")
            throw new CSideException(string.Format("Field no. {0} is not of type Boolean", FieldNo));
         SetValue(value.ToString(), validate);
      }

      /// <summary>
      /// Validates the specified value.
      /// </summary>
      /// <param name="value">The value.</param>
      public void Validate(string value)
      {
         SetValue(value, true);
      }

      /// <summary>
      /// Validates the specified value.
      /// </summary>
      /// <param name="value">The value.</param>
      public void Validate(DateTime value)
      {
         SetValue(value, true);
      }

      /// <summary>
      /// Validates the specified value.
      /// </summary>
      /// <param name="value">The value.</param>
      public void Validate(int value)
      {
         SetValue(value, true);
      }

      /// <summary>
      /// Validates the specified value.
      /// </summary>
      /// <param name="value">The value.</param>
      public void Validate(decimal value)
      {
         SetValue(value, true);
      }

      /// <summary>
      /// Validates the specified value.
      /// </summary>
      /// <param name="value">if set to <c>true</c> [value].</param>
      public void Validate(bool value)
      {
         SetValue(value, true);
      }

		#endregion Methods 
   }
}
