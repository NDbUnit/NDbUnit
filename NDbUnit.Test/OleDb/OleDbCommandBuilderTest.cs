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
using System.Data;
using System.Data.OleDb;

using MbUnit.Framework;

using NDbUnit.Test;
using NDbUnit.Core.OleDb;

namespace NDbUnit.Test.OleDb
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	/// 
	[TestFixture]
	public class OleDbCommandBuilderTest
	{
		private NDbUnit.Core.OleDb.OleDbCommandBuilder _oleDbCommandBuilder = new NDbUnit.Core.OleDb.OleDbCommandBuilder(DbConnection.OleDbConnectionString);

		private bool emptyCommand(OleDbCommand oleDbCommand)
		{
			return (null == oleDbCommand || null == oleDbCommand.CommandText || "" == oleDbCommand.CommandText);
		}

		public OleDbCommandBuilderTest()
		{
			// needed when connecting to sql server
			_oleDbCommandBuilder.QuotePrefix = "[";
			_oleDbCommandBuilder.QuoteSuffix = "]";

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
			_oleDbCommandBuilder.BuildCommands(xsdFile);
		}

		[Test]
		public void TestGetSchema()
		{
			TestBuildCommands();
			
			_oleDbCommandBuilder.GetSchema();
		}

		[Test]
		public void TestGetSelectCommand()
		{
			TestBuildCommands();

			DataSet ds = _oleDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				OleDbCommand oleDbCommand = _oleDbCommandBuilder.GetSelectCommand(dataTable.TableName);

				Console.WriteLine("Table '" + dataTable.TableName + "' select command");
				Console.WriteLine("\t" + oleDbCommand.CommandText);
			}
		}

		[Test]
		public void TestGetInsertCommand()
		{
			TestBuildCommands();
			
			DataSet ds = _oleDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				OleDbCommand oleDbCommand = _oleDbCommandBuilder.GetInsertCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(oleDbCommand), "Insert command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' insert command");
				Console.WriteLine("\t" + oleDbCommand.CommandText);
			}
		}

		[Test]
		public void TestGetInsertIdentityCommand()
		{
			TestBuildCommands();
			
			DataSet ds = _oleDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				OleDbCommand oleDbCommand = _oleDbCommandBuilder.GetInsertIdentityCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(oleDbCommand), "Insert identity command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
				Console.WriteLine("\t" + oleDbCommand.CommandText);
			}
		}

		[Test]
		public void TestGetDeleteCommand()
		{
			TestBuildCommands();
			
			DataSet ds = _oleDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				OleDbCommand oleDbCommand = _oleDbCommandBuilder.GetDeleteCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(oleDbCommand), "Delete command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' delete command");
				Console.WriteLine("\t" + oleDbCommand.CommandText);
			}
		}

		[Test]
		public void TestGetDeleteAllCommand()
		{
			TestBuildCommands();
			
			DataSet ds = _oleDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				OleDbCommand oleDbCommand = _oleDbCommandBuilder.GetDeleteAllCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(oleDbCommand), "Delete command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' delete all command");
				Console.WriteLine("\t" + oleDbCommand.CommandText);
			}
		}

		[Test]
		public void TestGetUpdateCommand()
		{
			TestBuildCommands();
			
			DataSet ds = _oleDbCommandBuilder.GetSchema();
			foreach(DataTable dataTable in ds.Tables)
			{
				OleDbCommand oleDbCommand = _oleDbCommandBuilder.GetUpdateCommand(dataTable.TableName);
				Assert.IsTrue(!emptyCommand(oleDbCommand), "Update command was not set");

				Console.WriteLine("Table '" + dataTable.TableName + "' update command");
				Console.WriteLine("\t" + oleDbCommand.CommandText);
			}
		}
	}
}
