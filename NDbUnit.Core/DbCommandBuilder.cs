/*
 *
 * NDbUnit
 * Copyright (C)2005
 * http://www.ndbunit.org
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
using System.Xml;
using System.Data;
using System.Collections;
using System.Text;

namespace NDbUnit.Core
{
    public interface IDbCommandBuilder
    {
        string QuotePrefix
        {
            get;
            set;
        }

        string QuoteSuffix
        {
            get;
            set;
        }

        IDbConnection Connection
        {
            get;
        }

        DataSet GetSchema();
        void BuildCommands(string xmlSchemaFile);
        void BuildCommands(Stream xmlSchema);
        IDbCommand GetSelectCommand(string tableName);
        IDbCommand GetInsertCommand(string tableName);
        IDbCommand GetInsertIdentityCommand(string tableName);
        IDbCommand GetDeleteCommand(string tableName);
        IDbCommand GetDeleteAllCommand(string tableName);
        IDbCommand GetUpdateCommand(string tableName);
    }

    public abstract class DbCommandBuilder : IDbCommandBuilder
    {
        private Hashtable _dbCommandColl = new Hashtable();

        private bool _initialized = false;

        private string _quotePrefix = "";

        private string _quoteSuffix = "";

        private XmlDataDocument _xdd = new XmlDataDocument();

        private string _xmlSchemaFile = "";

        public DbCommandBuilder()
        {
        }

        IDbConnection IDbCommandBuilder.Connection
        {
            get
            {
                return GetConnection();
            }
        }

        public string QuotePrefix
        {
            get
            {
                return _quotePrefix;
            }

            set
            {
                _quotePrefix = value;
            }
        }

        public string QuoteSuffix
        {
            get
            {
                return _quoteSuffix;
            }

            set
            {
                _quoteSuffix = value;
            }
        }

        public string XmlSchemaFile
        {
            get
            {
                return _xmlSchemaFile;
            }
        }

        public void BuildCommands(Stream xmlSchema)
        {
            XmlDataDocument xdd = new XmlDataDocument();

            xdd.DataSet.ReadXmlSchema(xmlSchema);
            // DataSet table rows RowState property is set to Added
            // when read in from an xml file.
            xdd.DataSet.AcceptChanges();

            Hashtable ht = new Hashtable();

            Commands commands = null;
            foreach (DataTable dataTable in xdd.DataSet.Tables)
            {
                // Virtual overrides.
                commands = new Commands();
                commands.SelectCommand = CreateSelectCommand(xdd.DataSet, dataTable.TableName);
                commands.InsertCommand = CreateInsertCommand(commands.SelectCommand, dataTable.TableName);
                commands.InsertIdentityCommand = CreateInsertIdentityCommand(commands.SelectCommand, dataTable.TableName);
                commands.DeleteCommand = CreateDeleteCommand(commands.SelectCommand, dataTable.TableName);
                commands.DeleteAllCommand = CreateDeleteAllCommand(dataTable.TableName);
                commands.UpdateCommand = CreateUpdateCommand(commands.SelectCommand, dataTable.TableName);

                ht[dataTable.TableName] = commands;
            }

            _xdd = xdd;
            _dbCommandColl = ht;
            _initialized = true;
        }

        public void BuildCommands(string xmlSchemaFile)
        {
            Stream stream = null;
            try
            {
                stream = new FileStream(xmlSchemaFile, System.IO.FileMode.Open);
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

        public DataSet GetSchema()
        {
            isInitialized();
            return _xdd.DataSet;
        }

        protected abstract IDbCommand CreateDeleteAllCommand(string tableName);

        protected abstract IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName);

        protected abstract IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName);

        protected abstract IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName);

        protected abstract IDbCommand CreateSelectCommand(DataSet ds, string tableName);

        protected abstract IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName);

        protected abstract IDbConnection GetConnection();

     

        IDbCommand IDbCommandBuilder.GetDeleteAllCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).DeleteAllCommand;
        }

        IDbCommand IDbCommandBuilder.GetDeleteCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).DeleteCommand;
        }

        IDbCommand IDbCommandBuilder.GetInsertCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).InsertCommand;
        }

        IDbCommand IDbCommandBuilder.GetInsertIdentityCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).InsertIdentityCommand;
        }

        IDbCommand IDbCommandBuilder.GetSelectCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).SelectCommand;
        }

        IDbCommand IDbCommandBuilder.GetUpdateCommand(string tableName)
        {
            isInitialized();
            return ((Commands)_dbCommandColl[tableName]).UpdateCommand;
        }

        private void isInitialized()
        {
            if (!_initialized)
            {
                string message = "IDbCommandBuilder.BuildCommands(string) or IDbCommandBuilder.BuildCommands(Stream) must be called successfully";
                throw new NDbUnitException(message);
            }
        }

        private class Commands
        {
            public IDbCommand SelectCommand = null;
            public IDbCommand InsertCommand = null;
            public IDbCommand InsertIdentityCommand = null;
            public IDbCommand DeleteCommand = null;
            public IDbCommand DeleteAllCommand = null;
            public IDbCommand UpdateCommand = null;
        }

    }
}
