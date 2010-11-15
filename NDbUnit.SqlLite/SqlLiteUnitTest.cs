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

using System.Data.Common;
using System.Data.SQLite;
using System.Data;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteUnitTest : NDbUnitTest
    {
        public SqlLiteUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        public SqlLiteUnitTest(IDbConnection connection)
            : base(connection)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SQLiteDataAdapter((SQLiteCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection)
        {
            return new SqlLiteDbCommandBuilder(connection);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
        {
            return new SqlLiteDbCommandBuilder(connectionString);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new SqlLiteDbOperation();
        }

    }
}
