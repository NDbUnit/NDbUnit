/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2009
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
using MySql.Data.MySqlClient;
using System.Text;

namespace NDbUnit.Core.MySqlClient
{
    public class MySqlDbCommandBuilder : DbCommandBuilder
    {
        public MySqlDbCommandBuilder(string connectionString)
            : base(connectionString)
        {
        }

        public override string QuotePrefix
        {
            get { return ""; }
        }

        public override string QuoteSuffix
        {
            get { return ""; }
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        protected override IDbCommand CreateDbCommand()
        {
            return new MySqlCommand();
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new MySqlParameter("?p" + index, (MySqlDbType) dataRow["ProviderType"],
                                    (int) dataRow["ColumnSize"], (string) dataRow["ColumnName"]);
        }

        protected override string GetParameterDesignator(int count)
        {
            return "?p" + count;
        }
        protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            IDataParameter sqlParameter;
            IDbCommand sqlInsertCommand = CreateDbCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                
                    if (notFirstColumn)
                    {
                        sb.Append(", ");
                        sbParam.Append(", ");
                    }

                    notFirstColumn = true;

                    sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                    sbParam.Append(GetParameterDesignator(count));

                    sqlParameter = CreateNewSqlParameter(count, dataRow);
                    sqlInsertCommand.Parameters.Add(sqlParameter);

                    ++count;
                
            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertCommand.CommandText = sb.ToString();

            return sqlInsertCommand;
        }

    }
}
