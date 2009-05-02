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
using MySql.Data.MySqlClient;
using MbUnit.Framework;
using NDbUnit.Core.MySqlClient;
using System.Collections.Generic;

namespace NDbUnit.Test.Mysql
{
    [TestFixture]
    public class MySqlDbCommandBuilderTest
    {
        private MySqlDbCommandBuilder _mySqlDbCommandBuilder = new MySqlDbCommandBuilder(DbConnection.MysqlConnectionString);

        private bool IsEmptyCommand(IDbCommand mySqlCommand)
        {
            return (null == mySqlCommand || string.IsNullOrEmpty(mySqlCommand.CommandText));
        }

        [TearDown]
        public void TearDown()
        {
            Console.Out.Flush();
        }

        [Test]
        public void TestBuildCommands()
        {
            string xsdFile = XmlTestFiles.MySqlTestFiles.XmlSchemaFile;
            _mySqlDbCommandBuilder.BuildCommands(xsdFile);
        }

        [Test]
        public void TestGetSchema()
        {
            TestBuildCommands();

            _mySqlDbCommandBuilder.GetSchema();
        }

        [Test]
        public void TestGetSelectCommand()
        {
            TestBuildCommands();

            DataSet ds = _mySqlDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand mySqlCommand = _mySqlDbCommandBuilder.GetSelectCommand(dataTable.TableName);

                Console.WriteLine("Table '" + dataTable.TableName + "' select command");
                Console.WriteLine("\t" + mySqlCommand.CommandText);
            }
        }

        [Test]
        public void TestGetInsertCommand()
        {
            TestBuildCommands();

            DataSet ds = _mySqlDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand mySqlCommand = _mySqlDbCommandBuilder.GetInsertCommand( dataTable.TableName);
                Assert.IsTrue(!IsEmptyCommand(mySqlCommand), "Insert command was not set");

                Console.WriteLine("Table '" + dataTable.TableName + "' insert command");
                Console.WriteLine("\t" + mySqlCommand.CommandText);
            }
        }

        [Test]
        public void TestGetInsertIdentityCommand()
        {
            TestBuildCommands();

            DataSet ds = _mySqlDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand mySqlCommand = _mySqlDbCommandBuilder.GetInsertIdentityCommand(dataTable.TableName);
                Assert.IsTrue(!IsEmptyCommand(mySqlCommand), "Insert identity command was not set");

                Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
                Console.WriteLine("\t" + mySqlCommand.CommandText);
            }
        }

        [Test]
        public void TestGetDeleteCommand()
        {
            TestBuildCommands();

            DataSet ds = _mySqlDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand mySqlCommand = _mySqlDbCommandBuilder.GetDeleteCommand(dataTable.TableName);
                Assert.IsTrue(!IsEmptyCommand(mySqlCommand), "Delete command was not set");

                Console.WriteLine("Table '" + dataTable.TableName + "' delete command");
                Console.WriteLine("\t" + mySqlCommand.CommandText);
            }
        }

        [Test]
        public void TestGetDeleteAllCommand()
        {
            TestBuildCommands();

            DataSet ds = _mySqlDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand mySqlCommand = _mySqlDbCommandBuilder.GetDeleteAllCommand(dataTable.TableName);
                Assert.IsTrue(!IsEmptyCommand(mySqlCommand), "Delete all command was not set");

                Console.WriteLine("Table '" + dataTable.TableName + "' delete all command");
                Console.WriteLine("\t" + mySqlCommand.CommandText);
            }
        }

        [Test]
        public void TestGetUpdateCommand()
        {
            TestBuildCommands();

            DataSet ds = _mySqlDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand mySqlCommand = _mySqlDbCommandBuilder.GetUpdateCommand(dataTable.TableName);
                Assert.IsTrue(!IsEmptyCommand(mySqlCommand), "Update command was not set");

                Console.WriteLine("Table '" + dataTable.TableName + "' update command");
                Console.WriteLine("\t" + mySqlCommand.CommandText);
            }
        }
    }
}

