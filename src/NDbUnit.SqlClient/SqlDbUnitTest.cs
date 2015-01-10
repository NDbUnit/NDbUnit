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

using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace NDbUnit.Core.SqlClient
{
    /// <summary>
    /// The Sql Server unit test data adapter.
    /// </summary>
    /// <example>
    /// <code>
    /// string connectionString = "Persist Security Info=False;Integrated Security=SSPI;database=testdb;server=V-AL-DIMEOLA\NETSDK";
    /// SqlDbUnitTest sqlDbUnitTest = new SqlDbUnitTest(connectionString);
    /// string xmlSchemaFile = "User.xsd";
    /// string xmlFile = "User.xml";
    /// sqlDbUnitTest.ReadXmlSchema(xmlSchemaFile);
    /// sqlDbUnitTest.ReadXml(xmlFile);
    /// sqlDbUnitTest.PerformDbOperation(DbOperation.CleanInsertIdentity);
    /// </code>
    /// <seealso cref="INDbUnitTest"/>
    /// </example>
    public class SqlDbUnitTest : NDbUnitTest<SqlConnection>
    {
        public SqlDbUnitTest(SqlConnection connection)
            : base(connection)
        {
        }

        public SqlDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SqlConnection> connectionManager )
        {
            SqlDbCommandBuilder commandBuilder = new SqlDbCommandBuilder(connectionManager);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        //protected override IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection)
        //{
        //    SqlDbCommandBuilder commandBuilder = new SqlDbCommandBuilder(connection);
        //    commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
        //    return commandBuilder;
        //}

        protected override IDbOperation CreateDbOperation()
        {
            return new SqlDbOperation();
        }

    }
}
