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
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteDbCommandBuilder : DbCommandBuilder
    {
        private new DataTable _dataTableSchema;

        public SqlLiteDbCommandBuilder(string connectionString)
            : base(connectionString)
        {
        }

        public SqlLiteDbCommandBuilder(IDbConnection connection)
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
            var command = new SQLiteCommand();

            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDbCommand CreateDeleteAllCommand(string tableName)
        {
            return
                new SQLiteCommand("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));
        }

        protected override IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " WHERE ");

            SQLiteCommand sqlDeleteCommand = CreateDbCommand() as SQLiteCommand;

            int count = 1;
            DbParameter sqlParameter;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    // A key column.
                    if ((bool)dataRow[SchemaColumns.IsKey])
                    {
                        if (count != 1)
                        {
                            sb.Append(" AND ");
                        }

                        sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                        sb.Append("=@p" + count);

                        sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
                        sqlDeleteCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }
                }
            }

            sqlDeleteCommand.CommandText = sb.ToString();

            return sqlDeleteCommand;
        }

        protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            DbParameter sqlParameter = null;
            SQLiteCommand sqlInsertCommand = CreateDbCommand() as SQLiteCommand;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    // Not an identity column.
                    if (!IsAutoIncrementing(dataRow))
                    {
                        if (notFirstColumn)
                        {
                            sb.Append(", ");
                            sbParam.Append(", ");
                        }

                        notFirstColumn = true;

                        sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                        sbParam.Append("@p" + count);

                        sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
                        sqlInsertCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }
                }
            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertCommand.CommandText = sb.ToString();

            return sqlInsertCommand;
        }

        protected override IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            DbParameter sqlParameter = null;
            SQLiteCommand sqlInsertIdentityCommand = CreateDbCommand() as SQLiteCommand;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    if (notFirstColumn)
                    {
                        sb.Append(", ");
                        sbParam.Append(", ");
                    }

                    notFirstColumn = true;

                    sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                    sbParam.Append("@p" + count);

                    sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
                    sqlInsertIdentityCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertIdentityCommand.CommandText = sb.ToString();

            return sqlInsertIdentityCommand;
        }


        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new SQLiteParameter("@p" + index, (DbType)dataRow[SchemaColumns.ProviderType],
                                       (int)dataRow[SchemaColumns.ColumnSize],
                                       (string)dataRow[SchemaColumns.ColumnName]);
        }

        protected override IDbCommand CreateSelectCommand(DataSet ds, string tableName)
        {
            SQLiteCommand sqlSelectCommand = CreateDbCommand() as SQLiteCommand;

            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder("SELECT ");
            DataTable dataTable = ds.Tables[tableName];
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                if (notFirstColumn)
                {
                    sb.Append(", ");
                }

                notFirstColumn = true;

                sb.Append(QuotePrefix + dataColumn.ColumnName + QuoteSuffix);
            }

            sb.Append(" FROM ");
            sb.Append(TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));

            sqlSelectCommand.CommandText = sb.ToString();
            sqlSelectCommand.Connection = (SQLiteConnection)_sqlConnection;

            try
            {
                _dataTableSchema = GetSchemaTable(sqlSelectCommand);
            }
            catch (Exception e)
            {
                string message =
                    String.Format(
                        "SqlDbCommandBuilder.CreateSelectCommand(DataSet, string) failed for tableName = '{0}'",
                        tableName);
                throw new NDbUnitException(message, e);
            }

            return sqlSelectCommand;
        }

        protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " SET ");

            SQLiteCommand sqlUpdateCommand = CreateDbCommand() as SQLiteCommand;

            int count = 1;
            bool notFirstKey = false;
            bool notFirstColumn = false;
            DbParameter sqlParameter = null;
            StringBuilder sbPrimaryKey = new StringBuilder();

            bool containsAllPrimaryKeys = true;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (!(bool)dataRow[SchemaColumns.IsKey])
                {
                    containsAllPrimaryKeys = false;
                    break;
                }
            }

            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    // A key column.
                    if ((bool)dataRow[SchemaColumns.IsKey])
                    {
                        if (notFirstKey)
                        {
                            sbPrimaryKey.Append(" AND ");
                        }

                        notFirstKey = true;

                        sbPrimaryKey.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                        sbPrimaryKey.Append("=@p" + count);

                        sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
                        sqlUpdateCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }


                    if (containsAllPrimaryKeys || !(bool)dataRow[SchemaColumns.IsKey])
                    {
                        if (notFirstColumn)
                        {
                            sb.Append(", ");
                        }

                        notFirstColumn = true;

                        sb.Append(QuotePrefix + dataRow[SchemaColumns.ColumnName] + QuoteSuffix);
                        sb.Append("=@p" + count);

                        sqlParameter = (SQLiteParameter)CreateNewSqlParameter(count, dataRow);
                        sqlUpdateCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }
                }
            }

            sb.Append(" WHERE " + sbPrimaryKey);

            sqlUpdateCommand.CommandText = sb.ToString();

            return sqlUpdateCommand;
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new SQLiteConnection(connectionString);
        }

        /// <summary>
        /// Since SQLite keys are auto incremented by default we need to check to see if the column
        /// is a key as well, since not all columns will be marked with AUTOINCREMENT
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsAutoIncrementing(DataRow row)
        {
            return (bool)row[SchemaColumns.IsAutoIncrement];
        }

        private class SchemaColumns
        {
            public const string ColumnName = "ColumnName";
            public const string ColumnOrdinal = "ColumnOrdinal";
            public const string ColumnSize = "ColumnSize";
            public const string NumericalPrecision = "NumericalPrecision";
            public const string NumericalScale = "NumericalScale";
            public const string IsUnique = "IsUnique";
            public const string IsKey = "IsKey";
            public const string BaseServerName = "BaseServerName";
            public const string BaseCatalogName = "BaseCatalogName";
            public const string BaseColumnName = "BaseColumnName";
            public const string BaseSchemaName = "";
            public const string IsAutoIncrement = "IsAutoIncrement";
            public const string ProviderType = "ProviderType";

        }

    }
}
