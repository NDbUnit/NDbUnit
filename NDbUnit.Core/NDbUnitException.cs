/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2011
 * http://code.google.com/p/ndbunit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using System.Runtime.Serialization;

namespace NDbUnit.Core
{
	/// <summary>
	/// The base class exception of all exceptions thrown by objects
	/// in NDbUnit.
	/// </summary>
	[Serializable]
	public class NDbUnitException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class.
		/// </summary>
		public NDbUnitException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class 
		/// with a specified error message.
		/// </summary>
		/// <param name="message"></param>
		public NDbUnitException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class 
		/// with serialized data.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected NDbUnitException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NDbUnitException"/> class 
		/// with the specified error message and a reference to the inner exception 
		/// that is the cause of this exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public NDbUnitException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
