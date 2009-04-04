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
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace NDbUnit.Core.OleDb
{
    public class OleDbCommandBuilder : DbCommandBuilder
    {
        private OleDbConnection _oleDbConnection;

        public OleDbCommandBuilder(string connectionString) : base(connectionString)
        {
            _oleDbConnection = new OleDbConnection(connectionString);
        }

        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        public new OleDbConnection Connection
        {
            get { return _oleDbConnection; }
        }


        protected override DbConnection GetConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        protected override DbCommand CreateDbCommand()
        {
            return new OleDbCommand();
        }

        protected override string GetParameterDesignator(int count)
        {
            return "?";
        }

        protected override string GetIdentityColumnDesignator()
        {
            return "IsAutoIncrement";
        }

        protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " SET ");

			OleDbCommand oleDbUpdateCommand = new OleDbCommand();

            int count = 1;
            bool notFirstKey = false;
            bool notFirstColumn = false;
            DbParameter oleDbParameter;
            StringBuilder sbPrimaryKey = new StringBuilder();
            ArrayList keyParameters = new ArrayList();

            bool containsAllPrimaryKeys = true;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (!(bool) dataRow["IsKey"])
                {
                    containsAllPrimaryKeys = false;
                    break;
                }
            }

            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                // A key column.
                if ((bool) dataRow["IsKey"])
                {
                    if (notFirstKey)
                    {
                        sbPrimaryKey.Append(" AND ");
                    }

					notFirstKey = true;

                    sbPrimaryKey.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                    sbPrimaryKey.Append("=?");

                    oleDbParameter = CreateNewSqlParameter(count, dataRow);
                    keyParameters.Add(oleDbParameter);

					++count;
				}

				if (containsAllPrimaryKeys || !(bool)dataRow["IsKey"])
				{
					if (notFirstColumn)
					{
						sb.Append(", ");
					}

					notFirstColumn = true;

                    sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                    sb.Append("=?");

                    oleDbParameter = CreateNewSqlParameter(count, dataRow);
                    oleDbUpdateCommand.Parameters.Add(oleDbParameter);

                    ++count;
                }
            }

			// Add key parameters last since ordering is important.
			for (int i = 0; i < keyParameters.Count; ++i)
			{
				oleDbUpdateCommand.Parameters.Add((OleDbParameter)keyParameters[i]);
			}

            sb.Append(" WHERE " + sbPrimaryKey);

			oleDbUpdateCommand.CommandText = sb.ToString();

			return oleDbUpdateCommand;
		}

        protected override DbParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new OleDbParameter("@p" + index, (System.Data.OleDb.OleDbType) dataRow["ProviderType"],
                                      (int) dataRow["ColumnSize"], (string) dataRow["ColumnName"]);
        }
    }
}
