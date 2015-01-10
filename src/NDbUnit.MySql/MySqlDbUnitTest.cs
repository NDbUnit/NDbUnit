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
using MySql.Data.MySqlClient;
using System.Data;

namespace NDbUnit.Core.MySqlClient
{
    /// <summary>
    /// The MySql unit test data adapter.
    /// </summary>
    /// <example>
    /// <code>
    /// string connectionString = "Persist Security Info=False;Integrated Security=SSPI;database=testdb;server=V-AL-DIMEOLA\NETSDK";
    /// MySqlDbUnitTest sqlDbUnitTest = new SqlDbUnitTest(connectionString);
    /// string xmlSchemaFile = "User.xsd";
    /// string xmlFile = "User.xml";
    /// sqlDbUnitTest.ReadXmlSchema(xmlSchemaFile);
    /// sqlDbUnitTest.ReadXml(xmlFile);
    /// sqlDbUnitTest.PerformDbOperation(DbOperation.CleanInsertIdentity);
    /// </code>
    /// <seealso cref="INDbUnitTest"/>
    /// </example>
    public class MySqlDbUnitTest : NDbUnitTest<MySqlConnection>
    {
        public MySqlDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        public MySqlDbUnitTest(MySqlConnection connection)
            : base(connection)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return null;
            //return new MySqlDataAdapter((MySqlCommand) command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<MySqlConnection> connectionManager)
        {
            return new MySqlDbCommandBuilder(connectionManager);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new MySqlDbOperation();
        }

        protected override void OnGetDataSetFromDb(string tableName, ref DataSet dsToFill, IDbConnection dbConnection)
        {
            MySqlCommand selectCommand = (MySqlCommand)GetDbCommandBuilder().GetSelectCommand(tableName);
            selectCommand.Connection = dbConnection as MySqlConnection;
            MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand);
            adapter.Fill(dsToFill, tableName);
        }

        public override void ExecuteScripts()
        {
            var connection = ConnectionManager.GetConnection();

            if (connection.State != ConnectionState.Open)
                connection.Open();

            foreach (string ddlText in ScriptManager.ScriptContents)
            {
                var script = new MySqlScript(connection, ddlText);
                script.Execute();
            }


            if (connection.State != ConnectionState.Closed)
                connection.Close();

        }
    }
}
