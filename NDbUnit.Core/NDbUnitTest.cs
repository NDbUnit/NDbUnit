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
using System.Data.Common;
using System.IO;
using System.Data;
using System.Collections.Specialized;

namespace NDbUnit.Core
{
    public class OperationEventArgs : EventArgs
    {
        public IDbTransaction DbTransaction { get; set; }
    }

    public delegate void PreOperationEvent(object sender, OperationEventArgs args);

    public delegate void PostOperationEvent(object sender, OperationEventArgs args);

    /// <summary>
    /// The base class implementation of all NDbUnit unit test data adapters.
    /// </summary>
    public abstract class NDbUnitTest : INDbUnitTest
    {
        private IDbConnection _connection;

        private string _connectionString;

        private IDbCommandBuilder _dbCommandBuilder;

        private readonly IDbOperation _dbOperation;

        private DataSet _dataSet;

        private bool _initialized;

        private bool _passedconnection;

        private string _xmlFile = "";

        private string _xmlSchemaFile = "";

        public event PostOperationEvent PostOperation;

        public event PreOperationEvent PreOperation;

        protected NDbUnitTest(IDbConnection connection)
        {
            _passedconnection = true;
            _connection = connection;
            _connectionString = connection.ConnectionString;
            _dbOperation = CreateDbOperation();

        }

        protected NDbUnitTest(string connectionString)
        {
            _connectionString = connectionString;
            _dbOperation = CreateDbOperation();

        }

        public int CommandTimeOut { get; set; }

        protected virtual DataSet DS
        {
            get { return _dataSet; }
        }

        //Todo: remove method at some point
        public DataSet CopyDataSet()
        {
            checkInitialized();
            return _dataSet.Copy();
        }

        //Todo: remove method at some point  
        public DataSet CopySchema()
        {
            checkInitialized();
            return _dataSet.Clone();
        }

        public DataSet GetDataSetFromDb(StringCollection tableNames)
        {
            checkInitialized();

            IDbCommandBuilder dbCommandBuilder = GetDbCommandBuilder();
            if (null == tableNames)
            {
                tableNames = new StringCollection();
                foreach (DataTable dt in _dataSet.Tables)
                {
                    tableNames.Add(dt.TableName);
                }
            }

            IDbConnection dbConnection = dbCommandBuilder.Connection;
            try
            {
                dbConnection.Open();
                DataSet dsToFill = _dataSet.Clone();
                foreach (string tableName in tableNames)
                {
                    OnGetDataSetFromDb(tableName, ref dsToFill, dbConnection);
                }

                return dsToFill;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (ConnectionState.Open == dbConnection.State)
                {
                    dbConnection.Close();
                }
            }
        }

        public DataSet GetDataSetFromDb()
        {
            return GetDataSetFromDb(null);
        }

        public void PerformDbOperation(DbOperationFlag dbOperationFlag)
        {
            checkInitialized();

            if (dbOperationFlag == DbOperationFlag.None)
            {
                return;
            }

            IDbCommandBuilder dbCommandBuilder = GetDbCommandBuilder();
            IDbOperation dbOperation = GetDbOperation();

            IDbTransaction dbTransaction = null;
            IDbConnection dbConnection = dbCommandBuilder.Connection;

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
                dbTransaction = dbConnection.BeginTransaction();

                OperationEventArgs args = new OperationEventArgs();
                args.DbTransaction = dbTransaction;

                if (null != PreOperation)
                {
                    PreOperation(this, args);
                }

                switch (dbOperationFlag)
                {
                    case DbOperationFlag.Insert:
                        {
                            dbOperation.Insert(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.InsertIdentity:
                        {
                            dbOperation.InsertIdentity(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.Delete:
                        {
                            dbOperation.Delete(_dataSet, dbCommandBuilder, dbTransaction);

                            break;
                        }
                    case DbOperationFlag.DeleteAll:
                        {
                            dbOperation.DeleteAll(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.Refresh:
                        {
                            dbOperation.Refresh(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.Update:
                        {
                            dbOperation.Update(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.CleanInsert:
                        {
                            dbOperation.DeleteAll(_dataSet, dbCommandBuilder, dbTransaction);
                            dbOperation.Insert(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                    case DbOperationFlag.CleanInsertIdentity:
                        {
                            dbOperation.DeleteAll(_dataSet, dbCommandBuilder, dbTransaction);
                            dbOperation.InsertIdentity(_dataSet, dbCommandBuilder, dbTransaction);
                            break;
                        }
                }

                if (null != PostOperation)
                {
                    PostOperation(this, args);
                }

                dbTransaction.Commit();
            }
            catch (Exception)
            {
                if (dbTransaction != null)
                {
                    dbTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (!_passedconnection)
                {
                    if (ConnectionState.Open == dbConnection.State)
                    {
                        dbConnection.Close();
                    }
                }
            }
        }

        public void ReadXml(string xmlFile)
        {
            if (string.IsNullOrEmpty(xmlFile))
            {
                throw new ArgumentException("Xml file cannot be null or empty", "xmlFile");
            }

            if (XmlDataFileHasNotYetBeenRead(xmlFile))
            {
                Stream stream = null;
                try
                {
                    stream = GetXmlDataFileStream(xmlFile);
                    ReadXml(stream);                    
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                _xmlFile = xmlFile;
            }
        }

        public void ReadXml(Stream xml)
        {
            if (_dataSet == null)
            {
                throw new InvalidOperationException("You must first call ReadXmlSchema before reading in xml data.");
            }
            _dataSet.Clear();
            _dataSet.ReadXml(xml);
        }

        public void ReadXmlSchema(string xmlSchemaFile)
        {
            if (string.IsNullOrEmpty(xmlSchemaFile))
            {
                throw new ArgumentException("Schema file cannot be null or empty", "xmlSchemaFile");
            }

            if (XmlSchemaFileHasNotYetBeenRead(xmlSchemaFile))
            {
                Stream stream = null;
                try
                {
                    stream = GetXmlSchemaFileStream(xmlSchemaFile);
                    ReadXmlSchema(stream);
                }
                finally
                {
                    if (stream != null)
                    {
                        stream.Close();
                    }
                }
                _xmlSchemaFile = xmlSchemaFile;
            }

            _initialized = true;
        }

        public void ReadXmlSchema(Stream xmlSchema)
        {
            IDbCommandBuilder dbCommandBuilder = GetDbCommandBuilder();
            dbCommandBuilder.BuildCommands(xmlSchema);

            DataSet dsSchema = dbCommandBuilder.GetSchema();

            _dataSet = dsSchema.Clone();

            _initialized = true;
        }

        protected abstract IDbDataAdapter CreateDataAdapter(IDbCommand command);

        protected abstract IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection);

        protected abstract IDbCommandBuilder CreateDbCommandBuilder(string connectionString);

        protected abstract IDbOperation CreateDbOperation();

        protected IDbCommandBuilder GetDbCommandBuilder()
        {
            if (_dbCommandBuilder == null)

                if (_connection == null)
                {
                    _dbCommandBuilder = CreateDbCommandBuilder(_connectionString);
                }
                else
                {
                    _dbCommandBuilder = CreateDbCommandBuilder(_connection);
                }

            return _dbCommandBuilder;
        }

        protected IDbOperation GetDbOperation()
        {
            return _dbOperation;
        }

        protected virtual FileStream GetXmlDataFileStream(string xmlFile)
        {
            return new FileStream(xmlFile, FileMode.Open);
        }

        protected virtual FileStream GetXmlSchemaFileStream(string xmlSchemaFile)
        {
            return new FileStream(xmlSchemaFile, FileMode.Open,
                                  FileAccess.Read, FileShare.Read);
        }

        protected virtual void OnGetDataSetFromDb(string tableName, ref DataSet dsToFill, IDbConnection dbConnection)
        {
            IDbCommand selectCommand = GetDbCommandBuilder().GetSelectCommand(tableName);
            selectCommand.Connection = dbConnection;
            IDbDataAdapter adapter = CreateDataAdapter(selectCommand);
            ((DbDataAdapter)adapter).Fill(dsToFill, tableName);
        }

        private void checkInitialized()
        {
            if (!_initialized)
            {
                string message =
                    "INDbUnitTest.ReadXmlSchema(string) or INDbUnitTest.ReadXmlSchema(Stream) must be called successfully";
                throw new NDbUnitException(message);
            }
        }

        private bool XmlDataFileHasNotYetBeenRead(string xmlFile)
        {
            return _xmlFile.ToLower() != xmlFile.ToLower();
        }

        private bool XmlSchemaFileHasNotYetBeenRead(string xmlSchemaFile)
        {
            return _xmlSchemaFile.ToLower() != xmlSchemaFile.ToLower();
        }

    }
}
