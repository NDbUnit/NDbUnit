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

using System;
using System.Data;
using System.Data.SqlServerCe;
using NDbUnit.Core.SqlServerCe;
using MbUnit.Framework;

namespace NDbUnit.Test.SqlServerCe
{
    [TestFixture]
    public class SqlCeDbOperationTest
    {
        private SqlCeDbOperation dbOperation;
        private SqlCeDbCommandBuilder commandBuilder;
        private DataSet _dsData;
        private string xmlFile;

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            commandBuilder = new SqlCeDbCommandBuilder(DbConnection.SqlCeConnectionString);

            string xmlSchemaFile = XmlTestFiles.XmlSchemaFileForSqlServerCe;
            xmlFile = XmlTestFiles.XmlFile;

            try
            {
                commandBuilder.BuildCommands(xmlSchemaFile);
            }
            catch (Exception e)
            {
                throw (e);
            }

            DataSet dsSchema = commandBuilder.GetSchema();
            _dsData = dsSchema.Clone();
            _dsData.ReadXml(xmlFile);
        }

        [SetUp]
        public void SetUp()
        {
            dbOperation = new SqlCeDbOperation();
            commandBuilder.Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            commandBuilder.Connection.Close();
        }

        [Test]
        public void TestInsert()
        {
            ResetIdentityColumns();

            TestDeleteAll();

            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                dbOperation.Insert(_dsData, commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestInsertIdentity()
        {
            TestDeleteAll();

            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                dbOperation.InsertIdentity(_dsData, commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestDeleteAll()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                dbOperation.DeleteAll(commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestDelete()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                dbOperation.Delete(_dsData, commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestUpdate()
        {
            TestDeleteAll();
            TestInsert();

            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = commandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.XmlModFile;
                ds.ReadXml(xmlFile);

                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                dbOperation.Update(ds, commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestRefresh()
        {
            TestDeleteAll();
            TestInsert();

            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = commandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.XmlRefreshFile;
                ds.ReadXml(xmlFile);

                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                dbOperation.Refresh(ds, commandBuilder, sqlTransaction);
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        private void ResetIdentityColumns()
        {
            IDbTransaction sqlTransaction = null;
            try
            {
                DataSet dsSchema = commandBuilder.GetSchema();
                sqlTransaction = commandBuilder.Connection.BeginTransaction();
                foreach (DataTable table in dsSchema.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.AutoIncrement)
                        {
                            String sql = "ALTER TABLE [" + table.TableName + "] ALTER COLUMN [" + column.ColumnName +
                                         "] IDENTITY (1,1)";
                            SqlCeCommand sqlCommand = new SqlCeCommand(sql, (SqlCeConnection) commandBuilder.Connection);
                            sqlCommand.Transaction = (SqlCeTransaction) sqlTransaction;
                            sqlCommand.ExecuteNonQuery();

                            break;
                        }
                    }
                }
                sqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (sqlTransaction != null)
                {
                    sqlTransaction.Rollback();
                }

                throw (e);
            }
        }
    }
}