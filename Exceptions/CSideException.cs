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

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using Org.Edgerunner.Dynamics.Nav.CSide.Interfaces;

namespace Org.Edgerunner.Dynamics.Nav.CSide.Exceptions
{
   /// <summary>
   /// A class representing a C/Side exception
   /// </summary>
   [Serializable]
   public class CSideException : Exception
   {
      #region Constructors/Deconstructors (4) 

      /// <summary>
      /// Initializes a new instance of the <see cref="CSideException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="innerException">The inner exception.</param>
      public CSideException(string message, Exception innerException)
         : base(message, innerException)
      {         
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CSideException"/> class.
      /// </summary>
      /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
      /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
      /// <exception cref="T:System.ArgumentNullException">
      /// The <paramref name="info"/> parameter is null.
      /// </exception>
      /// <exception cref="T:System.Runtime.Serialization.SerializationException">
      /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
      /// </exception>
      protected CSideException(SerializationInfo info, StreamingContext context)
         : base(info, context)
      {         
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CSideException"/> class.
      /// </summary>
      /// <param name="message">The message.</param>
      public CSideException(string message)
         : base(message)
      {         
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CSideException"/> class.
      /// </summary>
      public CSideException()
      {         
      }

      #endregion Constructors/Deconstructors 

      #region Methods (1) 

      // Static Methods (1) 

      /// <summary>
      /// Gets the exception corresponding to the hresult.
      /// </summary>
      /// <param name="errorCode">The error code.</param>
      /// <returns>A <see cref="CSideException"/>.</returns>
      /// <remarks>If the error code is 0 then an Exception is thrown stating that it is not a valid error</remarks>
      /// <exception cref="T:System.ArgumentException">The error code was invalid.</exception>
      public static CSideException GetException(int errorCode)
      {
         if (errorCode == 0)
            throw new ArgumentException(string.Format("{0} is not a valid error code", errorCode));

         Exception innerException = Marshal.GetExceptionForHR(errorCode); 
         return new CSideException(innerException.Message, innerException);
      }

      #endregion Methods 

      /// <summary>
      /// Obtains the error information pointer set by the previous call to SetErrorInfo in the current logical thread.
      /// </summary>
      /// <param name="reserved">Reserved for future use. Must be zero.</param>
      /// <param name="errorInfo">Pointer to a pointer to an error object.</param>
      /// <returns>The return value obtained from the returned HRESULT is either S_OK if an error was obtained or S_FALSE if no error exists</returns>
      [DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
      private static extern int GetErrorInfo(int reserved, [MarshalAs(UnmanagedType.Interface)] out IErrorInfo errorInfo);
   }
}
