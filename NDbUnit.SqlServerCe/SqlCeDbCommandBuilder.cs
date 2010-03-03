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
using System.Data.SqlServerCe;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeDbCommandBuilder : DbCommandBuilder
    {
        public SqlCeDbCommandBuilder(string connectionString)
            : base(connectionString)
        {
        }

        public SqlCeDbCommandBuilder(IDbConnection connection)
            : base(connection)
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
            SqlCeCommand command = new SqlCeCommand();

            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new SqlCeParameter("@p" + index, ((SqlCeType)dataRow["ProviderType"]).SqlDbType,
                                      (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new SqlCeConnection(connectionString);
        }

    }
}