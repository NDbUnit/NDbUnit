/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2009
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
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using System.Data;

namespace NDbUnit.Test.Common
{
    public abstract class DbCommandBuilderTestBase
    {
        private const int EXPECTED_COUNT_OF_COMMANDS = 3;

        private IDbCommandBuilder _commandBuilder;

        public abstract IList<string> ExpectedDataSetTableNames { get; }

        public abstract IList<string> ExpectedDeleteAllCommands { get; }

        public abstract IList<string> ExpectedDeleteCommands { get; }

        public abstract IList<string> ExpectedInsertCommands { get; }

        public abstract IList<string> ExpectedInsertIdentityCommands { get; }

        public abstract IList<string> ExpectedSelectCommands { get; }

        public abstract IList<string> ExpectedUpdateCommands { get; }

        [SetUp]
        public void _SetUp()
        {
            _commandBuilder = GetDbCommandBuilder();
            _commandBuilder.BuildCommands(GetXmlSchemaFilename());

        }

        [Test]
        [MultipleAsserts]
        public void GetDeleteAllCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();

            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = _commandBuilder.GetDeleteAllCommand(dataTable.TableName);
                commandList.Add(dbCommand.CommandText);

                Console.WriteLine("Table '" + dataTable.TableName + "' delete all command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedDeleteAllCommands, commandList);
        }

        [Test]
        [MultipleAsserts]
        public void GetDeleteCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();

            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = _commandBuilder.GetDeleteCommand(dataTable.TableName);
                commandList.Add(dbCommand.CommandText);

                Console.WriteLine("Table '" + dataTable.TableName + "' delete command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedDeleteCommands, commandList);
        }

        [Test]
        [MultipleAsserts]
        public void GetInsertCommand_Creates_Correct_SQL_Commands()
        {
            DataSet ds = _commandBuilder.GetSchema();
            IList<string> commandList = new List<string>();

            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = _commandBuilder.GetInsertCommand(dataTable.TableName);
                commandList.Add(dbCommand.CommandText);

                Console.WriteLine("Table '" + dataTable.TableName + "' insert command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedInsertCommands, commandList);
        }

        [Test]
        [MultipleAsserts]
        public void GetInsertIdentityCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();
            DataSet ds = _commandBuilder.GetSchema();

            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = _commandBuilder.GetInsertIdentityCommand(dataTable.TableName);
                commandList.Add(dbCommand.CommandText);

                Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedInsertIdentityCommands, commandList);
        }

        [Test]
        [MultipleAsserts]
        public void GetSchema_Contains_Proper_Tables()
        {
            IDbCommandBuilder builder = GetDbCommandBuilder();
            builder.BuildCommands(GetXmlSchemaFilename());
            DataSet schema = builder.GetSchema();

            IList<string> schemaTables = new List<string>();

            foreach (DataTable dataTable in schema.Tables)
            {
                schemaTables.Add(dataTable.TableName);

                Console.WriteLine("Table '" + dataTable.TableName + "' found in dataset");
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, schema.Tables.Count, string.Format("Should be {0} Tables in dataset", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedDataSetTableNames, schemaTables);
        }

        [Test]
        public void GetSchema_Throws_NDbUnit_Exception_When_Not_Initialized()
        {
            IDbCommandBuilder builder = GetDbCommandBuilder();
            try
            {
                builder.GetSchema();
                Assert.Fail("Expected Exception of type NDbUnitException not thrown!");
            }
            catch (NDbUnitException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        [MultipleAsserts]
        public void GetSelectCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();
            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = _commandBuilder.GetSelectCommand(dataTable.TableName);
                commandList.Add(dbCommand.CommandText);

                Console.WriteLine("Table '" + dataTable.TableName + "' select command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedSelectCommands, commandList);
        }

        [Test]
        [MultipleAsserts]
        public void GetUpdateCommand_Creates_Correct_SQL_Commands()
        {
            IList<string> commandList = new List<string>();

            DataSet ds = _commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = _commandBuilder.GetUpdateCommand(dataTable.TableName);
                commandList.Add(dbCommand.CommandText);

                Console.WriteLine("Table '" + dataTable.TableName + "' update command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, commandList.Count, string.Format("Should be {0} commands", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedUpdateCommands, commandList);
        }

        protected abstract IDbCommandBuilder GetDbCommandBuilder();

        protected abstract string GetXmlSchemaFilename();

    }
}
