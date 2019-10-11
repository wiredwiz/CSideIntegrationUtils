#region Apache License 2.0
// <copyright company="Edgerunner.org" file="VirtualTables.cs">
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

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// A static class for methods related to virtual tables.
   /// </summary>
   public static class VirtualTables
   {
      /// <summary>
      /// Attempts to lookup the name of a known virtual table by the supplied table id.
      /// </summary>
      /// <param name="tableId">The table number to fetch the name of.</param>
      /// <returns>A System.String that represents the table name if found, otherwise null.</returns>
      public static string TryGetName(int tableId)
      {
         switch (tableId)
         {
            case 2000000007: return "Date";
            case 2000000009: return "Session";
            case 2000000020: return "Drive";
            case 2000000022: return "File";
            case 2000000026: return "Integer";
            case 2000000028: return "Table Information";
            case 2000000029: return "System Object";
            case 2000000038: return "AllObj";
            case 2000000039: return "Printer";
            case 2000000040: return "License Information";
            case 2000000041: return "Field";
            case 2000000043: return "License Permission";
            case 2000000044: return "Permission Range";
            case 2000000045: return "Windows Language";
            case 2000000048: return "Database";
            case 2000000049: return "Code Coverage";
            case 2000000053: return "Access Control";
            case 2000000055: return "SID - Account ID";
            case 2000000058: return "AllObjWithCaption";
            case 2000000063: return "Key";
            case 2000000101: return "Debugger Call Stack";
            case 2000000102: return "Debugger Variable";
            case 2000000103: return "Debugger Watch";
            case 2000000135: return "Table Synch. Setup";
         }

         return null;
      }
   }
}