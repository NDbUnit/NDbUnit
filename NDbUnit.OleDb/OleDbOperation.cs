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
using System.Data.OleDb;

namespace NDbUnit.Core.OleDb
{
    public class OleDbOperation : DbOperation
    {
        private OleDbType _oleOleDbType = OleDbType.NoDb;

        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        public OleDbType OleOleDbType
        {
            get { return _oleOleDbType; }
            set { _oleOleDbType = value; }
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new OleDbDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new OleDbCommand(cmdText);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            if (_oleOleDbType == OleDbType.SqlServer)
            {
                base.OnInsertIdentity(dataTable, dbCommand, dbTransaction);
            }
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            if (_oleOleDbType != OleDbType.SqlServer) return;

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
            if (_oleOleDbType != OleDbType.SqlServer) return;

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
