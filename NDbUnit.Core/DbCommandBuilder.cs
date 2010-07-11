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
using System.IO;
using System.Text;
using System.Data;
using System.Collections;

namespace NDbUnit.Core
{
    public abstract class DbCommandBuilder : IDbCommandBuilder
    {
        private DataSet _dataSet = new DataSet();

        protected DataTable _dataTableSchema;

        private Hashtable _dbCommandColl = new Hashtable();

        private bool _initialized;

        private bool _passedconnection;

        protected IDbConnection _sqlConnection;

        private string _xmlSchemaFile = "";

        protected DbCommandBuilder(IDbConnection connection)
        {
            _passedconnection = true;
            _sqlConnection = connection;
        }

        protected DbCommandBuilder(string connectionString)
        {
            _sqlConnection = GetConnection(connectionString);
        }

        public int CommandTimeOutSeconds { get; set; }

        public IDbConnection Connection
        {
            get { return _sqlConnection; }
        }

        public virtual string QuotePrefix
        {
            get { return ""; }
        }

        public virtual string QuoteSuffix
        {
            get { return ""; }
        }

        public string XmlSchemaFile
        {
            get { return _xmlSchemaFile; }
        }

        public void BuildCommands(string xmlSchemaFile)
        {
            Stream stream = null;
            try
            {
                stream = new FileStream(xmlSchemaFile, FileMode.Open, FileAccess.Read);
                BuildCommands(stream);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            _xmlSchemaFile = xmlSchemaFile;
            _initialized = true;
        }

        public void BuildCommands(Stream xmlSchema)
        {

            _dataSet.ReadXmlSchema(xmlSchema);
            // DataSet table rows RowState property is set to Added
            // when read in from an xml file.
            _dataSet.AcceptChanges();

            Hashtable ht = new Hashtable();

            Commands commands;
            foreach (DataTable dataTable in _dataSet.Tables)
            {
                // Virtual overrides.
                commands = new Commands();
                commands.SelectCommand = CreateSelectCommand(_dataSet, dataTable.TableName);
                commands.InsertCommand = CreateInsertCommand(commands.SelectCommand, dataTable.TableName);
                commands.InsertIdentityCommand = CreateInsertIdentityCommand(commands.SelectCommand, dataTable.TableName);
                commands.DeleteCommand = CreateDeleteCommand(commands.SelectCommand, dataTable.TableName);
                commands.DeleteAllCommand = CreateDeleteAllCommand(dataTable.TableName);
                commands.UpdateCommand = CreateUpdateCommand(commands.SelectCommand, dataTable.TableName);

                ht[dataTable.TableName] = commands;
            }

            _dbCommandColl = ht;
            _initialized = true;
        }

        public IDbCommand GetDeleteAllCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).DeleteAllCommand;
        }

        public IDbCommand GetDeleteCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).DeleteCommand;
        }

        public IDbCommand GetInsertCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).InsertCommand;
        }

        public IDbCommand GetInsertIdentityCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).InsertIdentityCommand;
        }

        public DataSet GetSchema()
        {
            isInitialized();
            return _dataSet;
        }

        public IDbCommand GetSelectCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).SelectCommand;
        }

        public IDbCommand GetUpdateCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).UpdateCommand;
        }

        protected virtual bool ColumnOKToInclude(DataRow dataRow)
        {
            try
            {
                string columnName = (string)dataRow["ColumnName"];

                bool found = false;

                foreach (DataTable table in _dataSet.Tables)
                {
                    found = table.Columns.Contains(columnName);
                    if (found == true)
                        break;
                }

                return found && !(bool)dataRow["IsHidden"] && dataRow["DataTypeName"].ToString() != "timestamp";
            }
            catch (Exception)
            {
                //if we cannot determine a reason NOT to include the column, we have to assume its OK to do so
                return true;
            }
        }

        protected abstract IDbCommand CreateDbCommand();

        protected virtual IDbCommand CreateDeleteAllCommand(string tableName)
        {
            IDbCommand command = CreateDbCommand();
            command.CommandText = String.Format("DELETE FROM {0}", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix));
            return command;
        }

        protected virtual IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("DELETE FROM {0} WHERE ", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix)));

            IDbCommand sqlDeleteCommand = CreateDbCommand();

            int count = 1;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                // A key column.
                if ((bool)dataRow["IsKey"])
                {
                    if (count != 1)
                    {
                        sb.Append(" AND ");
                    }

                    sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                    sb.Append(String.Format("={0}", GetParameterDesignator(count)));

                    IDataParameter sqlParameter = CreateNewSqlParameter(count, dataRow);
                    sqlDeleteCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sqlDeleteCommand.CommandText = sb.ToString();

            return sqlDeleteCommand;
        }

        protected virtual IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("INSERT INTO {0}(", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix)));
            StringBuilder sbParam = new StringBuilder();
            IDataParameter sqlParameter;
            IDbCommand sqlInsertCommand = CreateDbCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    // Not an identity column.
                    if (!((bool)dataRow[GetIdentityColumnDesignator()]))
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
                }
            }

            sb.Append(String.Format(") VALUES({0})", sbParam));

            sqlInsertCommand.CommandText = sb.ToString();

            return sqlInsertCommand;
        }

        protected virtual IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("INSERT INTO {0}(", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix)));
            StringBuilder sbParam = new StringBuilder();
            IDataParameter sqlParameter;
            IDbCommand sqlInsertIdentityCommand = CreateDbCommand();
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

                    sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                    sbParam.Append(GetParameterDesignator(count));

                    sqlParameter = CreateNewSqlParameter(count, dataRow);
                    sqlInsertIdentityCommand.Parameters.Add(sqlParameter);

                    ++count;
                }
            }

            sb.Append(String.Format(") VALUES({0})", sbParam));

            sqlInsertIdentityCommand.CommandText = sb.ToString();

            return sqlInsertIdentityCommand;
        }

        protected abstract IDataParameter CreateNewSqlParameter(int index, DataRow dataRow);

        protected virtual IDbCommand CreateSelectCommand(DataSet ds, string tableName)
        {
            IDbCommand sqlSelectCommand = CreateDbCommand();

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
            sqlSelectCommand.Connection = _sqlConnection;

            try
            {
                _dataTableSchema = GetSchemaTable(sqlSelectCommand);
            }
            catch (Exception e)
            {
                string message =
                    String.Format(
                        "DbCommandBuilder.CreateSelectCommand(DataSet, string) failed for tableName = '{0}'",
                        tableName);
                throw new NDbUnitException(message, e);
            }

            return sqlSelectCommand;
        }

        protected virtual IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("UPDATE {0} SET ", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix)));

            IDbCommand sqlUpdateCommand = CreateDbCommand();

            int count = 1;
            bool notFirstKey = false;
            bool notFirstColumn = false;
            StringBuilder sbPrimaryKey = new StringBuilder();

            bool containsAllPrimaryKeys = true;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (!(bool)dataRow["IsKey"])
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
                    IDataParameter sqlParameter;
                    if ((bool)dataRow["IsKey"])
                    {
                        if (notFirstKey)
                        {
                            sbPrimaryKey.Append(" AND ");
                        }

                        notFirstKey = true;

                        sbPrimaryKey.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                        sbPrimaryKey.Append(String.Format("={0}", GetParameterDesignator(count)));

                        sqlParameter = CreateNewSqlParameter(count, dataRow);
                        sqlUpdateCommand.Parameters.Add(sqlParameter);

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
                        sb.Append(String.Format("={0}", GetParameterDesignator(count)));

                        sqlParameter = CreateNewSqlParameter(count, dataRow);
                        sqlUpdateCommand.Parameters.Add(sqlParameter);

                        ++count;
                    }
                }
            }

            sb.Append(String.Format(" WHERE {0}", sbPrimaryKey));

            sqlUpdateCommand.CommandText = sb.ToString();

            return sqlUpdateCommand;
        }

        protected abstract IDbConnection GetConnection(string connectionString);

        protected virtual string GetIdentityColumnDesignator()
        {
            return "IsIdentity";
        }

        protected virtual string GetParameterDesignator(int count)
        {
            return String.Format("@p{0}", count);
        }

        //private DataTable GetSchemaTable(IDbCommand sqlSelectCommand)
        protected virtual DataTable GetSchemaTable(IDbCommand sqlSelectCommand)
        {
            DataTable dataTableSchema = new DataTable();
            bool isClosed = ConnectionState.Closed == _sqlConnection.State;

            try
            {
                if (isClosed)
                {
                    _sqlConnection.Open();
                }

                IDataReader sqlDataReader = sqlSelectCommand.ExecuteReader(CommandBehavior.KeyInfo);
                dataTableSchema = sqlDataReader.GetSchemaTable();
                sqlSelectCommand.Cancel();
                sqlDataReader.Close();
            }
            catch (NotSupportedException)
            {
                //swallow this since .Close() op isn't supported on all DB targets (e.g., SQLCE)
            }
            finally
            {
                //Only close connection if connection was not passed to constructor
                if (!_passedconnection)
                {
                    if (_sqlConnection.State != ConnectionState.Closed)
                    {
                        _sqlConnection.Close();
                    }
                }
            }

            return dataTableSchema;
        }

        private void isInitialized()
        {
            if (!_initialized)
            {
                string message =
                    "IDbCommandBuilder.BuildCommands(string) or IDbCommandBuilder.BuildCommands(Stream) must be called successfully";
                throw new NDbUnitException(message);
            }
        }

        private class Commands
        {
            public IDbCommand SelectCommand;
            public IDbCommand InsertCommand;
            public IDbCommand InsertIdentityCommand;
            public IDbCommand DeleteCommand;
            public IDbCommand DeleteAllCommand;
            public IDbCommand UpdateCommand;
        }

    }
}
