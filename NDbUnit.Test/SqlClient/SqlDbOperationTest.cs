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
using MbUnit.Framework;
using NDbUnit.Core.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class SqlDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlDbCommandBuilder(DbConnection.SqlConnectionString);
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = String.Format("dbcc checkident([{0}], RESEED, 0)", table.TableName);
            return new SqlCommand(sql, (SqlConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServer.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.SqlServer.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.SqlServer.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServer.XmlSchemaFile;
        }

    }
}
