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
using NDbUnit.Core.SqlLite;
using System.Data.SQLite;
using System.Data;
using System.IO;
using NDbUnit.Core;
using System.Diagnostics;

namespace NDbUnit.Test.SqlLite_InMemory
{
    [TestFixture]
    public class SQLliteInMemoryIntegrationTest
    {
        private SQLiteConnection _connection;

        [FixtureSetUp]
        public void _TestFixtureSetUp()
        {
            _connection = new SQLiteConnection(DbConnection.SqlLiteInMemConnectionString);
            ExecuteSchemaCreationScript();
        }

        [Test]
        [MultipleAsserts]
        public void Can_Get_Data_From_In_Memory_Instance()
        {
            var database = new SqlLiteDbUnitTest(_connection);

            database.ReadXmlSchema(XmlTestFiles.Sqlite.XmlSchemaFile);
            database.ReadXml(XmlTestFiles.Sqlite.XmlFile);

            database.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);

            var command = _connection.CreateCommand();
            command.CommandText = "Select * from [Role]";

            var results = command.ExecuteReader();
            
            Assert.IsTrue(results.HasRows);

            int recordCount = 0;

            while (results.Read())
            {
                recordCount++;
                Debug.WriteLine(results.GetString(1));
            }

            Assert.AreEqual(2, recordCount);

        }

        private void ExecuteSchemaCreationScript()
        {
            IDbCommand command = _connection.CreateCommand();
            command.CommandText = ReadTextFromFile(@"..\..\scripts\sqlite-testdb-create.sql");

            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            command.ExecuteNonQuery();

            command.CommandText = "Select * from Role";
            command.ExecuteReader();
        }

        private string ReadTextFromFile(string filename)
        {
            using (var sr = new StreamReader(filename))
            {
                return sr.ReadToEnd();
            }
        }

    }
}
