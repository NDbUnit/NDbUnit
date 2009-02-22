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

namespace NDbUnit.Test
{
	/// <summary>
	/// Summary description for DbConnection.
	/// </summary>
	public class DbConnection
	{
		public static string SqlConnectionString
		{
			get
			{
				return @"Data Source=127.0.0.1\sqlserver2005;Network Library=DBMSSOCN;Initial Catalog=testdb;Trusted_Connection=True";
			}
		}

		public static string OleDbConnectionString
		{
			get
			{
				return @"Provider=SQLOLEDB;Data Source=127.0.0.1\sqlserver2005;Network Library=DBMSSOCN;Initial Catalog=testdb;Integrated Security=SSPI";
			}
		}
	}
}
