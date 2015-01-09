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
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeDbUnitTest : NDbUnitTest<SqlCeConnection>
    {
        public SqlCeDbUnitTest(SqlCeConnection connection)
            : base(connection)
        {
        }

        public SqlCeDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlCeDataAdapter((SqlCeCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SqlCeConnection> connectionManager)
        {
            return new SqlCeDbCommandBuilder(connectionManager);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new SqlCeDbOperation();
        }

    }

    [Obsolete("Use SqlCeDbUnitTest class in place of this.")]
    public class SqlCeUnitTest : SqlCeDbUnitTest
    {
        public SqlCeUnitTest(SqlCeConnection connection) : base(connection)
        {
        }

        public SqlCeUnitTest(string connectionString) : base(connectionString)
        {
        }
    }
}
