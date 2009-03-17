/*
 *
 * NDbUnit
 * Copyright (C)2005
 * http://code.google.com/p/ndbunit
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
using System.Data;
using System.Data.SqlClient;

using MbUnit.Framework;

using NDbUnit.Test;
using NDbUnit.Core.SqlClient;

namespace NDbUnit.Test.SqlClient
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	/// 
	[TestFixture]
	public class SqlDbCommandBuilderTest
	{
		private SqlDbCommandBuilder _sqlDbCommandBuilder = new SqlDbCommandBuilder(DbConnection.SqlConnectionString);

		private bool emptyCommand(SqlCommand sqlCommand)
		{
			return (null == sqlCommand || null == sqlCommand.CommandText || "" == sqlCommand.CommandText);
		}

		public SqlDbCommandBuilderTest()
		{
		}

		[TearDown]
		public void TearDown()
		{
			Console.Out.Flush();
		}

		[Test]
		public void TestBuildCommands()
		{
			string xsdFile = XmlTestFiles.XmlSchemaFile;
			_sqlDbCommandBuilder.BuildCommands(xsdFile);
		}

		[Test]
		public void TestGetSchema()
		{
			TestBuildCommands();
			
			_sqlDbCommandBuilder.GetSchema();
		}

		[Test]
		public void TestGetSelectCommand()
		{
			TestBuildCommands();

			DataSet ds = _sqlDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				SqlCommand sqlCommand = _sqlDbCommandBuilder.GetSelectCommand(dataTable.TableName);

				Console.WriteLine("Table '" + dataTable.TableName + "' select command");
				Console.WriteLine("\t" + sqlCommand.CommandText);
			}
		}

		[Test]
		public void TestGetInsertCommand()
		{
			TestBuildCommands();

			DataSet ds = _sqlDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				SqlCommand sqlCommand = _sqlDbCommandBuilder.GetInsertCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(sqlCommand), "Insert command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' insert command");
				Console.WriteLine("\t" + sqlCommand.CommandText);
			}
		}

		[Test]
		public void TestGetInsertIdentityCommand()
		{
			TestBuildCommands();

			DataSet ds = _sqlDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				SqlCommand sqlCommand = _sqlDbCommandBuilder.GetInsertIdentityCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(sqlCommand), "Insert identity command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
				Console.WriteLine("\t" + sqlCommand.CommandText);
			}
		}

		[Test]
		public void TestGetDeleteCommand()
		{
			TestBuildCommands();

			DataSet ds = _sqlDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				SqlCommand sqlCommand = _sqlDbCommandBuilder.GetDeleteCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(sqlCommand), "Delete command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' delete command");
				Console.WriteLine("\t" + sqlCommand.CommandText);
			}
		}

		[Test]
		public void TestGetDeleteAllCommand()
		{
			TestBuildCommands();

			DataSet ds = _sqlDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				SqlCommand sqlCommand = _sqlDbCommandBuilder.GetDeleteAllCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(sqlCommand), "Delete all command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' delete all command");
				Console.WriteLine("\t" + sqlCommand.CommandText);
			}
		}

		[Test]
		public void TestGetUpdateCommand()
		{
			TestBuildCommands();

			DataSet ds = _sqlDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				SqlCommand sqlCommand = _sqlDbCommandBuilder.GetUpdateCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(sqlCommand), "Update command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' update command");
				Console.WriteLine("\t" + sqlCommand.CommandText);
			}
		}
	}
}
