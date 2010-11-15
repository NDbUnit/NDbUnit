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

using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System;

namespace NDbUnit.Core.SqlClient
{
    public class SqlDbCommandBuilder : DbCommandBuilder
    {
        public SqlDbCommandBuilder(IDbConnection connection)
            : base(connection)
        {
        }

        public SqlDbCommandBuilder(string connectionString)
            : base(connectionString)
        {
        }

        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override IDbCommand CreateDbCommand()
        {
            SqlCommand command = new SqlCommand();
            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new SqlParameter("@p" + index, (SqlDbType)dataRow["ProviderType"],
                                    (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

    }
}
