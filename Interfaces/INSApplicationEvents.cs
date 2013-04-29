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
   [ComImport, Guid("50000004-0000-1000-0004-0000836BD2D2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
   internal interface INSApplicationEvents
   {
      /// <summary>
      /// Called when a form is opened within the client.
      /// </summary>
      /// <param name="form">The form.</param>
      /// <returns></returns>
      int OnFormOpen([In, MarshalAs(UnmanagedType.Interface)] INSForm form);
      /// <summary>
      /// As yet unknown.
      /// </summary>
      /// <param name="form">The form.</param>
      /// <param name="b">The b.</param>
      /// <returns></returns>
      int OnUnknown([In, MarshalAs(UnmanagedType.Interface)] INSForm form, [In] String b);
      /// <summary>
      /// Called when client active status changes.
      /// </summary>
      /// <param name="active">if set to <c>true</c> [active].</param>
      /// <returns></returns>
      int OnActiveChanged([In] bool active);
      /// <summary>
      /// Called when the company and/or database is changed.
      /// </summary>
      /// <returns></returns>
      int OnCompanyDBChange();
   }
}
