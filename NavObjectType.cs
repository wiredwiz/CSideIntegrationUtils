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
   /// <summary>
   /// Represents the different types of objects within Microsoft Dynamics Nav.
   /// </summary>
   public enum NavObjectType
   {
      /// <summary>
      /// A Codeunit
      /// </summary>
      Codeunit = 5,
      /// <summary>
      /// A Dataport
      /// </summary>
      Dataport = 4,
      /// <summary>
      /// A Form
      /// </summary>
      Form = 2,
      /// <summary>
      /// A Report
      /// </summary>
      Report = 3,
      /// <summary>
      /// A Table
      /// </summary>
      Table = 1,
      /// <summary>
      /// An XMLport
      /// </summary>
      XMLport = 6,
      /// <summary>
      /// A MenuSuite
      /// </summary>
      MenuSuite = 7,
      /// <summary>
      /// A Page
      /// </summary>
      Page = 8,
      /// <summary>
      /// A Query
      /// </summary>
      Query = 9
   }
}

