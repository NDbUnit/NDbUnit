/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2010
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
            var database = new SqlLiteUnitTest(_connection);

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
