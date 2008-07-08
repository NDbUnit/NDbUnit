/*
 *
 * NDbUnit
 * Copyright (C)2005
 * http://www.ndbunit.org
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
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
