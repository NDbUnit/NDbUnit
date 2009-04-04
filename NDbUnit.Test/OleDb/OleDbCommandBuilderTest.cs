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
using System.Collections.Generic;
using System.Data;
using NDbUnit.Core;
using NDbUnit.Core.OleDb;
using MbUnit.Framework;

namespace NDbUnit.Test.OleDb
{
    [TestFixture]
    public class OleDbCommandBuilderTest
    {
        private OleDbCommandBuilder _oleDbCommandBuilder;

        [SetUp]
        public void SetUp()
        {
            _oleDbCommandBuilder =
                new OleDbCommandBuilder(DbConnection.OleDbConnectionString);
            string xsdFile = XmlTestFiles.XmlSchemaFile;
            _oleDbCommandBuilder.BuildCommands(xsdFile);
        }

        [Test]
        [ExpectedException(typeof(NDbUnitException))]
        public void TestGetSchemaThrowsNDbUnitExceptionBecauseIsNotInitialized()
        {
            OleDbCommandBuilder builder = new OleDbCommandBuilder(DbConnection.OleDbConnectionString);
            builder.GetSchema();

		}

        [Test]
        public void TestGetSchema()
        {
            OleDbCommandBuilder builder = new OleDbCommandBuilder(DbConnection.OleDbConnectionString);
            builder.BuildCommands(XmlTestFiles.XmlSchemaFile);
            DataSet schema = builder.GetSchema();

            Assert.AreEqual(3, schema.Tables.Count, "Exptected 3 tables in dataset");
            Assert.AreEqual("Role", schema.Tables[0].TableName, "Wrong table name");
            Assert.AreEqual("dbo.User", schema.Tables[1].TableName, "Wrong table name");
            Assert.AreEqual("UserRole", schema.Tables[2].TableName, "Wrong table name");
        }

        [Test]
        public void TestGetSelectCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();
            DataSet ds = _oleDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand oleDbCommand = _oleDbCommandBuilder.GetSelectCommand(dataTable.TableName);
                commandList.Add(oleDbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("SELECT [ID], [Name], [Description] FROM [Role]", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("SELECT [ID], [FirstName], [LastName], [Age], [SupervisorID] FROM [dbo].[User]", commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("SELECT [UserID], [RoleID] FROM [UserRole]", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetInsertCommand()
        {
            DataSet ds = _oleDbCommandBuilder.GetSchema();
            List<IDbCommand> commandList = new List<IDbCommand>();

            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand oleDbCommand = _oleDbCommandBuilder.GetInsertCommand(dataTable.TableName);
                commandList.Add(oleDbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("INSERT INTO [Role]([Name], [Description]) VALUES(?, ?)", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("INSERT INTO [dbo].[User]([FirstName], [LastName], [Age], [SupervisorID]) VALUES(?, ?, ?, ?)",
                            commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(?, ?)", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetInsertIdentityCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();
            DataSet ds = _oleDbCommandBuilder.GetSchema();

            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand oleDbCommand = _oleDbCommandBuilder.GetInsertIdentityCommand(dataTable.TableName);
                commandList.Add(oleDbCommand);

                Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
                Console.WriteLine("\t" + oleDbCommand.CommandText);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("INSERT INTO [Role]([ID], [Name], [Description]) VALUES(?, ?, ?)",
                            commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("INSERT INTO [dbo].[User]([ID], [FirstName], [LastName], [Age], [SupervisorID]) VALUES(?, ?, ?, ?, ?)",
                            commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(?, ?)", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetDeleteCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();

            DataSet ds = _oleDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand oleDbCommand = _oleDbCommandBuilder.GetDeleteCommand(dataTable.TableName);
                commandList.Add(oleDbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("DELETE FROM [Role] WHERE [ID]=?", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [dbo].[User] WHERE [ID]=?", commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [UserRole] WHERE [UserID]=? AND [RoleID]=?", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetDeleteAllCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();

            DataSet ds = _oleDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand oleDbCommand = _oleDbCommandBuilder.GetDeleteAllCommand(dataTable.TableName);
                commandList.Add(oleDbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("DELETE FROM [Role]", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [dbo].[User]", commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [UserRole]", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetUpdateCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();

            DataSet ds = _oleDbCommandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand oleDbCommand = _oleDbCommandBuilder.GetUpdateCommand(dataTable.TableName);
                commandList.Add(oleDbCommand);

                Console.WriteLine("Table '" + dataTable.TableName + "' update command");
                Console.WriteLine("\t" + oleDbCommand.CommandText);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("UPDATE [Role] SET [Name]=?, [Description]=? WHERE [ID]=?", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("UPDATE [dbo].[User] SET [FirstName]=?, [LastName]=?, [Age]=?, [SupervisorID]=? WHERE [ID]=?",
                            commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("UPDATE [UserRole] SET [UserID]=?, [RoleID]=? WHERE [UserID]=? AND [RoleID]=?",
                            commandList[2].CommandText,
                            "Incorrect command text");
        }
    }
}
