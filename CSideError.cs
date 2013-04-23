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

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>A class that defines constants for the various error codes the client may return.</summary>
   public static class CSideError
   {
      public const int RPC_E_SERVERFAULT = -2147023174;
      public const int RPC_E_CALL_REJECTED = -2147418111;
      public const int RPC_E_SERVERCALL_RETRYLATER = -2147417846;
   }
}
