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
using System.Collections.Generic;
using System.Text;
using NDbUnit.Core;
using System.Data.OracleClient;
using System.Data;
using System.Data.Common;

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
