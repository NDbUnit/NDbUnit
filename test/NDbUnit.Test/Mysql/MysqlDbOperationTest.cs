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
using NDbUnit.Core.MySqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using NDbUnit.Core;
using NUnit.Framework;

namespace NDbUnit.Test.Mysql
{
    [Category(TestCategories.MySqlTests)]
    [TestFixture]
    class MySqlDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new MySqlDbCommandBuilder(new DbConnectionManager<MySqlConnection>(DbConnection.MySqlConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new MySqlDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1;";
            return new MySqlCommand(sql, (MySqlConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.MySql.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.MySql.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.MySql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

    }

}