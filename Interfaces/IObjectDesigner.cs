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
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
    [ComImport, Guid("50000004-0000-1000-0001-0000836BD2D2"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IObjectDesigner
    {
        [PreserveSig, DispId(1)]
        int ReadObject([In] int objectType, [In] int objectId, [In] IStream destination);
        [PreserveSig, DispId(2)]
        int ReadObjects([In] string filter, [In] IStream destination);
        [PreserveSig, DispId(3)]
        int WriteObjects([In] IStream source);
        [PreserveSig, DispId(4)]
        int CompileObject([In] int objectType, [In] int objectId);
        [PreserveSig, DispId(5)]
        int CompileObjects([In] string filter);
        [PreserveSig, DispId(6)]
        int GetServerName(out string serverName);
        [PreserveSig, DispId(7)]
        int GetDatabaseName(out string databaseName);
        [PreserveSig, DispId(8)]
        int GetServerType(out int serverType);
        [PreserveSig, DispId(9)]
        int GetCSIDEVersion(out string csideVersion);
        [PreserveSig, DispId(10)]
        int GetApplicationVersion(out string applicationVersion);
        [PreserveSig, DispId(11)]
        int GetCompanyName(out string companyName);
    }
}

