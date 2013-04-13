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

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   [ComImport, Guid("50000004-0000-1000-0003-0000836BD2D2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
   internal interface INSForm
   {
      int GetHyperlink(out String hyperlink);        // gets Hyperlink with Fieldcaptions
      int GetID(out String formId);               // gets active object's type (Form) and ID
      int GetRec([Out, MarshalAs(UnmanagedType.Interface)] out INSRec recod);
      int GetTable([Out, MarshalAs(UnmanagedType.Interface)] out INSTable table);
      int GetLanguageID(out int languageID);          // gets Language ID of application (1033, etc.)
      int GetButton([Out, MarshalAs(UnmanagedType.Interface)] out INSMenuButton menuButton); //never succeeded to call it correctly, each time end with error in NAV client...
      int proc9();
   }
}
