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
using NDbUnit.Core;
using NDbUnit.Core.SqlServerCe;
using MbUnit.Framework;
using System.Collections.Generic;

namespace NDbUnit.Test.SqlServerCe
{
    [TestFixture]
    public class SqlCeDbCommandBuilderTest
    {
        private SqlCeDbCommandBuilder commandBuilder;

        [SetUp]
        public void SetUp()
        {
            commandBuilder =
                new SqlCeDbCommandBuilder(DbConnection.SqlCeConnectionString);
            string xsdFile = XmlTestFiles.XmlSchemaFileForSqlServerCe;
            commandBuilder.BuildCommands(xsdFile);
        }

        [Test]
        [ExpectedException(typeof (NDbUnitException))]
        public void TestGetSchemaThrowsNDbUnitExceptionBecauseIsNotInitialized()
        {
            SqlCeDbCommandBuilder builder = new SqlCeDbCommandBuilder(DbConnection.SqlCeConnectionString);
            builder.GetSchema();
        }

        [Test]
        public void TestGetSchema()
        {
            SqlCeDbCommandBuilder builder = new SqlCeDbCommandBuilder(DbConnection.SqlCeConnectionString);
            builder.BuildCommands(XmlTestFiles.XmlSchemaFileForSqlServerCe);
            DataSet schema = builder.GetSchema();

            Assert.AreEqual(3, schema.Tables.Count, "Exptected 3 tables in dataset");
            Assert.AreEqual("Role", schema.Tables[0].TableName, "Wrong table name");
            Assert.AreEqual("User", schema.Tables[1].TableName, "Wrong table name");
            Assert.AreEqual("UserRole", schema.Tables[2].TableName, "Wrong table name");
        }

        [Test]
        public void TestGetSelectCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();
            DataSet ds = commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = commandBuilder.GetSelectCommand(dataTable.TableName);
                commandList.Add(dbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("SELECT [ID], [Name], [Description] FROM [Role]", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("SELECT [ID], [FirstName], [LastName], [Age], [SupervisorID] FROM [User]",
                            commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("SELECT [UserID], [RoleID] FROM [UserRole]", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetInsertCommand()
        {
            DataSet ds = commandBuilder.GetSchema();
            List<IDbCommand> commandList = new List<IDbCommand>();

            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = commandBuilder.GetInsertCommand(dataTable.TableName);
                commandList.Add(dbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("INSERT INTO [Role]([Name], [Description]) VALUES(@p1, @p2)", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual(
                "INSERT INTO [User]([FirstName], [LastName], [Age], [SupervisorID]) VALUES(@p1, @p2, @p3, @p4)",
                commandList[1].CommandText,
                "Incorrect command text");
            Assert.AreEqual("INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(@p1, @p2)", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetInsertIdentityCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();
            DataSet ds = commandBuilder.GetSchema();

            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = commandBuilder.GetInsertIdentityCommand(dataTable.TableName);
                commandList.Add(dbCommand);

                Console.WriteLine("Table '" + dataTable.TableName + "' insert identity command");
                Console.WriteLine("\t" + dbCommand.CommandText);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("INSERT INTO [Role]([ID], [Name], [Description]) VALUES(@p1, @p2, @p3)",
                            commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual(
                "INSERT INTO [User]([ID], [FirstName], [LastName], [Age], [SupervisorID]) VALUES(@p1, @p2, @p3, @p4, @p5)",
                commandList[1].CommandText,
                "Incorrect command text");
            Assert.AreEqual("INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(@p1, @p2)", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetDeleteCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();

            DataSet ds = commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = commandBuilder.GetDeleteCommand(dataTable.TableName);
                commandList.Add(dbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("DELETE FROM [Role] WHERE [ID]=@p1", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [User] WHERE [ID]=@p1", commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [UserRole] WHERE [UserID]=@p1 AND [RoleID]=@p2", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetDeleteAllCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();

            DataSet ds = commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = commandBuilder.GetDeleteAllCommand(dataTable.TableName);
                commandList.Add(dbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("DELETE FROM [Role]", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [User]", commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("DELETE FROM [UserRole]", commandList[2].CommandText,
                            "Incorrect command text");
        }

        [Test]
        public void TestGetUpdateCommand()
        {
            List<IDbCommand> commandList = new List<IDbCommand>();

            DataSet ds = commandBuilder.GetSchema();
            foreach (DataTable dataTable in ds.Tables)
            {
                IDbCommand dbCommand = commandBuilder.GetUpdateCommand(dataTable.TableName);
                commandList.Add(dbCommand);
            }

            Assert.AreEqual(3, commandList.Count, "Should be 3 commands");
            Assert.AreEqual("UPDATE [Role] SET [Name]=@p2, [Description]=@p3 WHERE [ID]=@p1", commandList[0].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("UPDATE [User] SET [FirstName]=@p2, [LastName]=@p3, [Age]=@p4, [SupervisorID]=@p5 WHERE [ID]=@p1",
                            commandList[1].CommandText,
                            "Incorrect command text");
            Assert.AreEqual("UPDATE [UserRole] SET [UserID]=@p2, [RoleID]=@p4 WHERE [UserID]=@p1 AND [RoleID]=@p3",
                            commandList[2].CommandText,
                            "Incorrect command text");
        }
    }
}