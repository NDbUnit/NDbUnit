/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2010
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
using System.Collections.Generic;
using System.Text;
using NDbUnit.Core.SqlClient;
using MbUnit.Framework;
using NDbUnit.Core;
using NDbUnit.Core.OleDb;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    class OleDbCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "dbo.User", "UserRole" 
                };
            }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM [Role]",
                    "DELETE FROM [dbo].[User]",
                    "DELETE FROM [UserRole]"
                };
            }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM [Role] WHERE [ID]=?",
                    "DELETE FROM [dbo].[User] WHERE [ID]=?",
                    "DELETE FROM [UserRole] WHERE [UserID]=? AND [RoleID]=?"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role]([Name], [Description]) VALUES(?, ?)",
                    "INSERT INTO [dbo].[User]([FirstName], [LastName], [Age], [SupervisorID]) VALUES(?, ?, ?, ?)",
                    "INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(?, ?)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role]([ID], [Name], [Description]) VALUES(?, ?, ?)",
                    "INSERT INTO [dbo].[User]([ID], [FirstName], [LastName], [Age], [SupervisorID]) VALUES(?, ?, ?, ?, ?)",
                    "INSERT INTO [UserRole]([UserID], [RoleID]) VALUES(?, ?)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT [ID], [Name], [Description] FROM [Role]",
                    "SELECT [ID], [FirstName], [LastName], [Age], [SupervisorID] FROM [dbo].[User]",
                    "SELECT [UserID], [RoleID] FROM [UserRole]"
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                {
                    "UPDATE [Role] SET [Name]=?, [Description]=? WHERE [ID]=?",
                    "UPDATE [dbo].[User] SET [FirstName]=?, [LastName]=?, [Age]=?, [SupervisorID]=? WHERE [ID]=?",
                    "UPDATE [UserRole] SET [UserID]=?, [RoleID]=? WHERE [UserID]=? AND [RoleID]=?"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new OleDbCommandBuilder(DbConnection.OleDbConnectionString);
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
