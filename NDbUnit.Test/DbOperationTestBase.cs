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

using System;
using System.Data;
using System.Data.SqlServerCe;
using NDbUnit.Core.SqlServerCe;
using MbUnit.Framework;
using NDbUnit.Core;
using System.Data.SqlClient;
using NDbUnit.Core.SqlClient;

namespace NDbUnit.Test.Common
{
    public abstract class DbOperationTestBase
    {
        protected IDbCommandBuilder _commandBuilder;

        protected IDbOperation _dbOperation;

        protected DataSet _dsData;

        protected string _xmlFile;

        [FixtureSetUp]
        public void _FixtureSetUp()
        {
            _commandBuilder = GetCommandBuilder();

            string xmlSchemaFile = GetXmlSchemaFilename();
            _xmlFile = GetXmlFilename();

            try
            {
                _commandBuilder.BuildCommands(xmlSchemaFile);
            }
            catch (Exception)
            {
                throw;
            }

            DataSet dsSchema = _commandBuilder.GetSchema();
            _dsData = dsSchema.Clone();
            _dsData.ReadXml(_xmlFile);

            _dbOperation = GetDbOperation();
        }

        [SetUp]
        public void _SetUp()
        {
            _commandBuilder.Connection.Open();
        }

        [TearDown]
        public void _TearDown()
        {
            _commandBuilder.Connection.Close();
        }

        [Test]
        public void Delete_Executes_Without_Exception()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                _dbOperation.Delete(_dsData, _commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void DeleteAll_Executes_Without_Exception()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                _dbOperation.DeleteAll(_commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void Insert_Executes_Without_Exception()
        {
            ResetIdentityColumns();

            DeleteAll_Executes_Without_Exception();

            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                _dbOperation.Insert(_dsData, _commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void InsertIdentity_Executes_Without_Exception()
        {
            DeleteAll_Executes_Without_Exception();

            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                _dbOperation.InsertIdentity(_dsData, _commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void Refresh_Executes_Without_Exception()
        {
            DeleteAll_Executes_Without_Exception();
            Insert_Executes_Without_Exception();

            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = _commandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = GetXmlRefeshFilename();
                ds.ReadXml(xmlFile);

                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                _dbOperation.Refresh(ds, _commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
            Assert.IsTrue(true);
        }

        [Test]
        public void Update_Executes_Without_Exception()
        {
            DeleteAll_Executes_Without_Exception();
            Insert_Executes_Without_Exception();

            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = _commandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.SqlServerCe.XmlModFile;
                ds.ReadXml(xmlFile);

                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                _dbOperation.Update(ds, _commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
            Assert.IsTrue(true);
        }

        protected abstract IDbCommandBuilder GetCommandBuilder();

        protected abstract IDbOperation GetDbOperation();

        protected abstract IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column);

        protected abstract string GetXmlFilename();

        protected abstract string GetXmlRefeshFilename();

        protected abstract string GetXmlSchemaFilename();

        protected void ResetIdentityColumns()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = _commandBuilder.GetSchema();
                sqlTransaction = _commandBuilder.Connection.BeginTransaction();
                foreach (DataTable table in dsSchema.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.AutoIncrement)
                        {
                            IDbCommand sqlCommand = GetResetIdentityColumnsDbCommand(table, column);
                            sqlCommand.Transaction = (IDbTransaction)sqlTransaction;
                            if (sqlCommand != null)
                                sqlCommand.ExecuteNonQuery();

                            break;
                        }
                    }
                }
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw;
            }
        }

    }
}