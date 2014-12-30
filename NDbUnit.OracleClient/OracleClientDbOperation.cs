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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NDbUnit.Core;
using Oracle.DataAccess.Client;

namespace NDbUnit.OracleClient
{
    public class OracleClientDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "\""; }
        }

        public override string QuoteSuffix
        {
            get { return QuotePrefix; }
        }

        protected override IDbCommand CreateDbCommand(string cmdText)
        {
            return new OracleCommand(cmdText);
        }

        protected override IDbDataAdapter CreateDbDataAdapter()
        {
            return new OracleDataAdapter();
        }

        protected override void DisableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            this.enableDisableTableConstraints("DISABLE", dataTable, dbTransaction);
        }

        protected override void EnableTableConstraints(DataTable dataTable, IDbTransaction dbTransaction)
        {
            this.enableDisableTableConstraints("ENABLE", dataTable, dbTransaction);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            throw new NotSupportedException("OnInsertIdentity not supported!");
        }

        private void enableDisableTableConstraints(String enableDisable, DataTable dataTable, IDbTransaction dbTransaction)
        {
            DbCommand dbCommand = null;
            DbParameter dbParam = null;
            DbDataReader dbReader = null;
            IList<String> altersList = new List<String>();

            String queryEnables =
                " SELECT 'ALTER TABLE '"
                + "    || table_name"
                + "    || ' " + enableDisable + " CONSTRAINT '"
                + "    || constraint_name AS alterComm"
                + "     FROM user_constraints"
                + "    WHERE UPPER(table_name) = UPPER(:tabela)"
                + "    AND constraint_type IN ('C', 'R')";

            dbCommand = new OracleCommand();
            dbCommand.CommandText = queryEnables;
            dbCommand.Connection = (DbConnection)dbTransaction.Connection;
            dbCommand.Transaction = (DbTransaction)dbTransaction;

            dbParam = new OracleParameter();
            dbParam.ParameterName = "tabela";
            dbParam.Value = dataTable.TableName;
            dbParam.DbType = DbType.String;
            dbCommand.Parameters.Add(dbParam);

            dbReader = dbCommand.ExecuteReader();
            while (dbReader.Read())
            {
                altersList.Add(dbReader.GetString(dbReader.GetOrdinal("alterComm")));
            }

            dbReader.Close();

            foreach (String returnedCommand in altersList)
            {

                var escapedCommand = returnedCommand.Replace(" " + dataTable.TableName + " ", TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix));

                dbCommand = new OracleCommand();
                dbCommand.CommandText = escapedCommand;
                dbCommand.Connection = (DbConnection)dbTransaction.Connection;
                dbCommand.Transaction = (DbTransaction)dbTransaction;
                dbCommand.ExecuteNonQuery();
            }
        }

    }
}
