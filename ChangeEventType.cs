﻿//
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
   /// <summary>
   /// Represents a type of change event triggered from Dynamics Nav
   /// </summary>
   public enum ChangeEventType
   {
      /// <summary>
      /// Database change event
      /// </summary>
      Database,
      /// <summary>
      /// Company change event
      /// </summary>
      Company,
      /// <summary>
      /// Server change event
      /// </summary>
      Server
   }
}
