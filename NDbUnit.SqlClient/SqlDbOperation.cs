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
using System.Data.SqlClient;
using System.Data;

namespace NDbUnit.Core.SqlClient
{
    public class SqlDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new SqlDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new SqlCommand(cmdText);
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            DbCommand sqlCommand =
                    (DbCommand)CreateDbCommand("ALTER TABLE " +
                                    TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) +
                                    " CHECK CONSTRAINT ALL");
            sqlCommand.Connection = (DbConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (DbTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();
        }

        protected override void DisableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            DbCommand sqlCommand =
                    (DbCommand)CreateDbCommand("ALTER TABLE " +
                                    TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) +
                                    " NOCHECK CONSTRAINT ALL");
            sqlCommand.Connection = (DbConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (DbTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();
        }
    }
}
