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
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Org.Edgerunner.Dynamics.Nav.CSide
{
   /// <summary>
   /// A class representing a C/Side exception
   /// </summary>
   public class CSideException : Exception
   {
		#region Constructors/Deconstructors (4) 

      /// <summary>
      /// Initializes a new instance of the <see cref="Org.Edgerunner.Dynamics.Nav.CSide.CSideException"/> class.
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

		// Static Methods (1) 

      /// <summary>
      /// Gets the exception corresponding to the hresult.
      /// </summary>
      /// <param name="hresult">The hresult.</param>
      /// <returns>A <see cref="System.Exception"/></returns>
      /// <remarks>If hresult is 0 then an Exception is thrown stating that it is not a valid error</remarks>
      public static Exception GetException(int hresult)
      {
         if (hresult == 0)
            throw new Exception(String.Format("{0} is not a valid error HRESULT", hresult));

         return Marshal.GetExceptionForHR(hresult);
      }

		#endregion Methods 

      /// <summary>
      /// Obtains the error information pointer set by the previous call to SetErrorInfo in the current logical thread.
      /// </summary>
      /// <param name="dwReserved">Reserved for future use. Must be zero.</param>
      /// <param name="ppIErrorInfo">Pointer to a pointer to an error object.</param>
      /// <returns>The return value obtained from the returned HRESULT is either S_OK if an error was obtained or S_FALSE if no error exists</returns>
      [DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
      private static extern int GetErrorInfo(int dwReserved, [MarshalAs(UnmanagedType.Interface)] out IErrorInfo ppIErrorInfo);
   }
}
