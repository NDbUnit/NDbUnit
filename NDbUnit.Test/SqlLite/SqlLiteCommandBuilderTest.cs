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
using NDbUnit.Core.SqlLite;
using MbUnit.Framework;
using NDbUnit.Core;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    class SqlLiteCommandBuilderTest : NDbUnit.Test.Common.DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "User", "UserRole" 
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
                    "DELETE FROM [User]",
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
                    "DELETE FROM [Role] WHERE [ID]=@p1",
                    "DELETE FROM [User] WHERE [ID]=@p1",
                    "DELETE FROM [UserRole] WHERE [UserID]=@p1 AND [RoleID]=@p2"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role](Name, Description) VALUES(@p1, @p2)",
                    "INSERT INTO [User](FirstName, LastName, Age, SupervisorID) VALUES(@p1, @p2, @p3, @p4)",
                    "INSERT INTO [UserRole](UserID, RoleID) VALUES(@p1, @p2)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO [Role](ID, Name, Description) VALUES(@p1, @p2, @p3)",
                    "INSERT INTO [User](ID, FirstName, LastName, Age, SupervisorID) VALUES(@p1, @p2, @p3, @p4, @p5)",
                    "INSERT INTO [UserRole](UserID, RoleID) VALUES(@p1, @p2)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT ID, Name, Description FROM [Role]",
                    "SELECT ID, FirstName, LastName, Age, SupervisorID FROM [User]",
                    "SELECT UserID, RoleID FROM [UserRole]"
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                {
                    "UPDATE [Role] SET Name=@p2, Description=@p3 WHERE ID=@p1",
                    "UPDATE [User] SET FirstName=@p2, LastName=@p3, Age=@p4, SupervisorID=@p5 WHERE ID=@p1",
                    "UPDATE [UserRole] SET UserID=@p2, RoleID=@p4 WHERE UserID=@p1 AND RoleID=@p3"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new SqlLiteDbCommandBuilder(DbConnection.SqlLiteConnectionString);
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Sqlite.XmlSchemaFile;
        }

    }
}
