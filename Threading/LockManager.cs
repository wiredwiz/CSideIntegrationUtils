#region Apache License 2.0
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
using System.Threading;

namespace Org.Edgerunner.Dynamics.Nav.CSide.Threading
{
   /// <summary>
   /// Class used for managing a smoother lock acquisition and release.
   /// </summary>
   public class LockManager : IDisposable
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="LockManager"/> class.
      /// </summary>
      /// <param name="lock">The lock.</param>
      public LockManager(object @lock)
      {
         Lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
      }

      /// <summary>
      /// Gets or sets the locking object.
      /// </summary>
      /// <value>The lock.</value>
      protected object Lock { get; set; }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      /// <exception cref="T:System.Threading.SynchronizationLockException">The current thread does not own the lock for the specified object.</exception>
      public void Dispose()
      {
         Monitor.Exit(Lock);
      }
   }
}