/*
 *
 * NDbUnit
 * Copyright (C)2005
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

using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace NDbUnit.Core.MySqlClient
{
    public class MySqlDbOperation : DbOperation
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
            return new MySqlDataAdapter();
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new MySqlCommand(cmdText);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            IDbTransaction sqlTransaction = (IDbTransaction)dbTransaction;

            try
            {
                DisableTableConstraints(dataTable, dbTransaction);

                IDbDataAdapter sqlDataAdapter = CreateDbDataAdapter();
                sqlDataAdapter.InsertCommand = dbCommand;
                sqlDataAdapter.InsertCommand.Connection = sqlTransaction.Connection;
                sqlDataAdapter.InsertCommand.Transaction = sqlTransaction;

                ((DbDataAdapter)sqlDataAdapter).Update(dataTable);

            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                EnableTableConstraints(dataTable, dbTransaction);                
            }
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            MySqlCommand sqlCommand = (MySqlCommand)CreateDbCommand("SET foreign_key_checks = 1;");
            sqlCommand.Connection = (MySqlConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (MySqlTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();
        }

        protected override void DisableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            MySqlCommand sqlCommand = (MySqlCommand)CreateDbCommand("SET foreign_key_checks = 0;");
            sqlCommand.Connection = (MySqlConnection)dbTransaction.Connection;
            sqlCommand.Transaction = (MySqlTransaction)dbTransaction;
            sqlCommand.ExecuteNonQuery();

        }
    }
}
