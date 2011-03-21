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
using System.Diagnostics;
using MbUnit.Framework;
using NDbUnit.Core;
using NDbUnit.Postgresql;
using NDbUnit.Test.Common;
using Npgsql;

namespace NDbUnit.Test.Postgresql
{
    [TestFixture]
    internal class PostgresqlDbOperationTest : DbOperationTestBase
    {
        protected override IDbCommandBuilder GetCommandBuilder()
        {
            return new PostgresqlDbCommandBuilder(DbConnection.PostgresqlConnectionString);
        }

        protected override IDbOperation GetDbOperation()
        {
            return new PostgresqlDbOperation();
        }


        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = string.Format("ALTER SEQUENCE \"{0}_{1}_seq\" RESTART WITH 1;", table.TableName,
                                       column.ColumnName);
            return new NpgsqlCommand(sql, (NpgsqlConnection) _commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Postgresql.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.Postgresql.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.Postgresql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Postgresql.XmlSchemaFile;
        }

        /*public override void InsertIdentity_Executes_Without_Exception()
        {
            Debug.WriteLine("InsertIdentity_Executes_Without_Exception Test skipped b/c InsertIdentity is unsupported by the Postgresql Provider at this time.");
        }*/
    }
}