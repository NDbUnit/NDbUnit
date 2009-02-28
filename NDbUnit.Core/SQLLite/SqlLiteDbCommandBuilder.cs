using System;
using System.Data;
using System.Data.SQLite;
using System.Text;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteDbCommandBuilder : DbCommandBuilder
    {
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


        #region Private Fields

        private readonly SQLiteConnection _sqlLiteConnection;
        private DataTable _dataTableSchema;

        #endregion

		#region Public Methods

		public SqlLiteDbCommandBuilder(string connectionString)
		{
		    _sqlLiteConnection = new SQLiteConnection(connectionString);

		    base.QuotePrefix = "[";
			base.QuoteSuffix = "]";
		}

		#endregion

        #region Type Safe Interface Implementation

        public SQLiteConnection Connection
        {
            get { return _sqlLiteConnection; }
        }

        public SQLiteCommand GetSelectCommand(string tableName)
        {
            return ((SQLiteCommand)((IDbCommandBuilder)this).GetSelectCommand(tableName));
        }

        public SQLiteCommand GetInsertCommand(string tableName)
        {
            return ((SQLiteCommand)((IDbCommandBuilder)this).GetInsertCommand(tableName));
        }

        public SQLiteCommand GetInsertIdentityCommand(string tableName)
        {
            return ((SQLiteCommand)((IDbCommandBuilder)this).GetInsertIdentityCommand(tableName));
        }

        public SQLiteCommand GetDeleteCommand(string tableName)
        {
            return ((SQLiteCommand)((IDbCommandBuilder)this).GetDeleteCommand(tableName));
        }

        public SQLiteCommand GetDeleteAllCommand(string tableName)
        {
            return ((SQLiteCommand)((IDbCommandBuilder)this).GetDeleteAllCommand(tableName));
        }

        public SQLiteCommand GetUpdateCommand(string tableName)
        {
            return ((SQLiteCommand)((IDbCommandBuilder)this).GetUpdateCommand(tableName));
        }

        #endregion

        #region Protected Overrides

        protected override IDbConnection GetConnection()
        {
            return _sqlLiteConnection;
        }

        protected override IDbCommand CreateSelectCommand(DataSet ds, string tableName)
        {
            SQLiteCommand sqlSelectCommand = new SQLiteCommand();

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

                sb.Append(base.QuotePrefix + dataColumn.ColumnName + base.QuoteSuffix);
            }

            sb.Append(" FROM ");
            sb.Append(TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));

            sqlSelectCommand.CommandText = sb.ToString();
            sqlSelectCommand.Connection = _sqlLiteConnection;

            try
            {
                _dataTableSchema = getSchemaTable(sqlSelectCommand);
            }
            catch (Exception e)
            {
                string message = String.Format("SqlDbCommandBuilder.CreateSelectCommand(DataSet, string) failed for tableName = '{0}'", tableName);
                throw new NDbUnitException(message, e);
            }

            return sqlSelectCommand;
        }

        /// <summary>
        /// Since SQLite keys are auto incremented by default we need to check to see if the column
        /// is a key as well, since not all columns will be marked with AUTOINCREMENT
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private bool IsAutoIncrementing(DataRow row)
        {
            return (bool) row[SchemaColumns.IsAutoIncrement];
        }

        protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + "(");
            StringBuilder sbParam = new StringBuilder();
            SQLiteParameter sqlParameter = null;
            SQLiteCommand sqlInsertCommand = new SQLiteCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                // Not an identity column.
                if (! IsAutoIncrementing(dataRow))
                {
                    if (notFirstColumn)
                    {
                        sb.Append(", ");
                        sbParam.Append(", ");
                    }

                    notFirstColumn = true;

                    sb.Append(base.QuotePrefix + dataRow[SchemaColumns.ColumnName] + base.QuoteSuffix);
                    sbParam.Append("@p" + count);

                    sqlParameter = newSqlParameter(count, dataRow);
                    sqlInsertCommand.Parameters.Add(sqlParameter);

                    ++count;
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
            SQLiteParameter sqlParameter = null;
            SQLiteCommand sqlInsertIdentityCommand = new SQLiteCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (notFirstColumn)
                {
                    sb.Append(", ");
                    sbParam.Append(", ");
                }

                notFirstColumn = true;

                sb.Append(base.QuotePrefix + dataRow[SchemaColumns.ColumnName] + base.QuoteSuffix);
                sbParam.Append("@p" + count);

                sqlParameter = newSqlParameter(count, dataRow);
                sqlInsertIdentityCommand.Parameters.Add(sqlParameter);

                ++count;
            }

            sb.Append(") VALUES(" + sbParam + ")");

            sqlInsertIdentityCommand.CommandText = sb.ToString();

            return sqlInsertIdentityCommand;
        }

        protected override IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " WHERE ");

            SQLiteCommand sqlDeleteCommand = new SQLiteCommand();

            int count = 1;
            SQLiteParameter sqlParameter = null;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                // A key column.
                if ((bool)dataRow[SchemaColumns.IsKey])
                {
                    if (count != 1)
                    {
                        sb.Append(" AND ");
                    }

                    sb.Append(base.QuotePrefix + dataRow[SchemaColumns.ColumnName] + base.QuoteSuffix);
                    sb.Append("=@p" + count.ToString());

                    sqlParameter = newSqlParameter(count, dataRow);
                    sqlDeleteCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sqlDeleteCommand.CommandText = sb.ToString();

            return sqlDeleteCommand;
        }

        protected override IDbCommand CreateDeleteAllCommand(string tableName)
        {
            return new SQLiteCommand("DELETE FROM " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));
        }

        protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " SET ");

            SQLiteCommand sqlUpdateCommand = new SQLiteCommand();

            int count = 1;
            bool notFirstKey = false;
            bool notFirstColumn = false;
            SQLiteParameter sqlParameter = null;
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
                // A key column.
                if ((bool)dataRow[SchemaColumns.IsKey])
                {
                    if (notFirstKey)
                    {
                        sbPrimaryKey.Append(" AND ");
                    }

                    notFirstKey = true;

                    sbPrimaryKey.Append(base.QuotePrefix + dataRow[SchemaColumns.ColumnName] + base.QuoteSuffix);
                    sbPrimaryKey.Append("=@p" + count);

                    sqlParameter = newSqlParameter(count, dataRow);
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

                    sb.Append(base.QuotePrefix + dataRow[SchemaColumns.ColumnName] + base.QuoteSuffix);
                    sb.Append("=@p" + count);

                    sqlParameter = newSqlParameter(count, dataRow);
                    sqlUpdateCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sb.Append(" WHERE " + sbPrimaryKey);

            sqlUpdateCommand.CommandText = sb.ToString();

            return sqlUpdateCommand;
        }

        #endregion

        #region Private Methods

        private DataTable getSchemaTable(SQLiteCommand sqlSelectCommand)
        {
            DataTable dataTableSchema = null;
            bool isClosed = ConnectionState.Closed == _sqlLiteConnection.State;

            try
            {
                if (isClosed)
                {
                    _sqlLiteConnection.Open();
                }

                SQLiteDataReader sqlDataReader = sqlSelectCommand.ExecuteReader(CommandBehavior.KeyInfo);
                dataTableSchema = sqlDataReader.GetSchemaTable();
                sqlDataReader.Close();
            }
            finally
            {
                if (isClosed)
                {
                    _sqlLiteConnection.Close();
                }
            }

            return dataTableSchema;
        }

        private SQLiteParameter newSqlParameter(int index, DataRow dataRow)
        {
            return new SQLiteParameter("@p" + index, (DbType)dataRow[SchemaColumns.ProviderType], (int)dataRow[SchemaColumns.ColumnSize], (string)dataRow[SchemaColumns.ColumnName]);
        }

        #endregion
    }
}
