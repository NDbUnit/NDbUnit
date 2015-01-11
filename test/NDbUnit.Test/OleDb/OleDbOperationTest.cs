/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2011
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
using System.Data;
using NDbUnit.Core.OleDb;
using System.Data.OleDb;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.OleDb
{
    [Category(TestCategories.OleDbTests)]
    [TestFixture]
    public class OleDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new NDbUnit.Core.OleDb.OleDbCommandBuilder(new DbConnectionManager<OleDbConnection>(DbConnection.OleDbConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new OleDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "dbcc checkident([" + table.TableName + "], RESEED, 0)";
            return new OleDbCommand(sql, (OleDbConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OleDb.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.OleDb.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.OleDb.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
