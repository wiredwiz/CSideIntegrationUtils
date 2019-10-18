#region Dansko LLC License
// <copyright>Copyright 2019 Thaddeus Ryker</copyright>
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

using System;
using System.Runtime.Serialization;

namespace Org.Edgerunner.Dynamics.Nav.CSide.Exceptions
{
   /// <inheritdoc />
   /// <summary>
   /// Class representing a CSide transaction exception.
   /// Implements the <see cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException" />
   /// </summary>
   /// <seealso cref="T:Org.Edgerunner.Dynamics.Nav.CSide.Exceptions.CSideException" />
   [Serializable]
   public class CSideTransactionException : CSideException
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CSideTransactionException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public CSideTransactionException(string message, Exception innerException)
         : base(message, innerException)
      {
      }
      
      /// <summary>
      /// Initializes a new instance of the <see cref="CSideTransactionException"/> class.
      /// </summary>
      public CSideTransactionException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CSideTransactionException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0).</exception>
      protected CSideTransactionException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CSideTransactionException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public CSideTransactionException(string message)
         : base(message)
      {
      }
   }
}