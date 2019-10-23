#region Apache License 2.0
// <copyright>Copyright 2010 Thaddeus Ryker</copyright>
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
#endregion

using System.Runtime.InteropServices;
using System.Security;

namespace Org.Edgerunner.Dynamics.Nav.CSide.Interfaces
{
   /// <summary>
   /// Interface IErrorInfo
   /// </summary>
   [ComImport, SuppressUnmanagedCodeSecurity, Guid("1CF2B120-547D-101B-8E65-08002B2BD119"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
   internal interface IErrorInfo
   {
      #region Operations (1)

      [PreserveSig]
      int GetGUID();

      #endregion Operations

      /// <summary>
      /// Gets the error source.
      /// </summary>
      /// <param name="errorSource">A string containing the source.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig]
      int GetSource([MarshalAs(UnmanagedType.BStr)] out string errorSource);

      /// <summary>
      /// Gets the description.
      /// </summary>
      /// <param name="errorDescription">A string containing the error description.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig]
      int GetDescription([MarshalAs(UnmanagedType.BStr)] out string errorDescription);
   }
}