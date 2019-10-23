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
using System.Runtime.InteropServices.ComTypes;

// ReSharper disable IdentifierTypo
namespace Org.Edgerunner.Dynamics.Nav.CSide.Interfaces
{
   /// <summary>
   /// Interface that defines methods for working with the object designer.
   /// </summary>
   [ComImport, Guid("50000004-0000-1000-0001-0000836BD2D2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IObjectDesigner
    {
      /// <summary>
      /// Reads the specified object from the connected database and into a stream.
      /// </summary>
      /// <param name="objectType">Type of the object.</param>
      /// <param name="objectId">The object number.</param>
      /// <param name="destination">The destination text stream.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      /// <remarks>ReadObject has bugs and should be avoided.  Instead, ReadObjects should be used</remarks>
      [PreserveSig, DispId(1)]
      int ReadObject([In] int objectType, [In] int objectId, [In] IStream destination);

      /// <summary>
      /// Reads the objects within the filter string from the connected database and into a stream.
      /// </summary>
      /// <param name="filter">The filter representing a subset of objects.</param>
      /// <param name="destination">The destination text stream.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(2)]
      int ReadObjects([In] string filter, [In] IStream destination);

      /// <summary>
      /// Writes a text stream containing C/AL objects to the connected database.
      /// </summary>
      /// <param name="source">The text stream containing C/AL text objects.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(3)]
      int WriteObjects([In] IStream source);

      /// <summary>
      /// Compiles the specified object in the connected database.
      /// </summary>
      /// <param name="objectType">Type of the object.</param>
      /// <param name="objectId">The object number.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(4)]
      int CompileObject([In] int objectType, [In] int objectId);

      /// <summary>
      /// Compiles the objects within the filter string in the connected database.
      /// </summary>
      /// <param name="filter">The filter representing a subset of objects.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(5)]
      int CompileObjects([In] string filter);

      /// <summary>
      /// Gets the name of the server the client is connected to.
      /// </summary>
      /// <param name="serverName">Name of the server.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(6)]
      int GetServerName(out string serverName);

      /// <summary>
      /// Gets the name of the database the client is connected to.
      /// </summary>
      /// <param name="databaseName">Name of the database.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(7)]
      int GetDatabaseName(out string databaseName);

      /// <summary>
      /// Gets the type of the server the client is connected to.
      /// </summary>
      /// <param name="serverType">Type of the server.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(8)]
      int GetServerType(out int serverType);

      /// <summary>
      /// Gets the C/Side version.
      /// </summary>
      /// <param name="csideVersion">The C/Side version.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(9)]
      int GetCSIDEVersion(out string csideVersion);

      /// <summary>
      /// Gets the application version.
      /// </summary>
      /// <param name="applicationVersion">The application version.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(10)]
      int GetApplicationVersion(out string applicationVersion);

      /// <summary>
      /// Gets the name of the company.
      /// </summary>
      /// <param name="companyName">Name of the company.</param>
      /// <returns>An integer value of either 0, indicating success or a non-zero error code for failure.</returns>
      [PreserveSig, DispId(11)]
      int GetCompanyName(out string companyName);
    }
}
