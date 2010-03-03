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
