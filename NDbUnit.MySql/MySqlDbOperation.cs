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
            catch (Exception)
            {
                throw;
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
