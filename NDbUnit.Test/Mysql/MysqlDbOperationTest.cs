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
using MySql.Data.MySqlClient;
using MbUnit.Framework;
using NDbUnit.Core.MySqlClient;

namespace NDbUnit.Test.Mysql
{
    [TestFixture]
    public class MysqlDbOperationTest
    {
        private bool _built = false;
        private MySqlDbOperation _mysqlDbOperation = new MySqlDbOperation();
        private MySqlDbCommandBuilder _mysqlDbCommandBuilder = new MySqlDbCommandBuilder(DbConnection.MysqlConnectionString);
        private DataSet _dsData = null;

        [SetUp]
        public void SetUp()
        {
            if (false == _built)
            {
                string xmlSchemaFile = XmlTestFiles.MySql.XmlSchemaFile;
                string xmlFile = XmlTestFiles.MySql.XmlFile;

                try
                {
                    _mysqlDbCommandBuilder.BuildCommands(xmlSchemaFile);
                }
                catch (Exception e)
                {
                    throw (e);
                }

                _built = true;

                DataSet dsSchema = _mysqlDbCommandBuilder.GetSchema();
                _dsData = dsSchema.Clone();
                _dsData.ReadXml(xmlFile);
            }

            _mysqlDbCommandBuilder.Connection.Open();
        }


        [TearDown]
        public void TearDown()
        {
            _mysqlDbCommandBuilder.Connection.Close();
        }

        [Test]
        public void TestInsert()
        {
            resetIdentityColumns();

            TestDeleteAll();

            IDbTransaction mysqlTransaction = null;
            try
            {
                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                _mysqlDbOperation.Insert(_dsData, _mysqlDbCommandBuilder, mysqlTransaction);
                mysqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestInsertIdentity()
        {
            TestDeleteAll();

            IDbTransaction mysqlTransaction = null;
            try
            {
                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                _mysqlDbOperation.InsertIdentity(_dsData, _mysqlDbCommandBuilder, mysqlTransaction);
                mysqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestDeleteAll()
        {
            IDbTransaction mysqlTransaction = null;
            try
            {
                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                _mysqlDbOperation.DeleteAll(_mysqlDbCommandBuilder, mysqlTransaction);
                mysqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestDelete()
        {
            IDbTransaction mysqlTransaction = null;
            try
            {
                //_mysqlDbCommandBuilder.Connection.Open();
                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                _mysqlDbOperation.Delete(_dsData, _mysqlDbCommandBuilder, mysqlTransaction);
                mysqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestUpdate()
        {
            TestDeleteAll();
            TestInsert();

            IDbTransaction mysqlTransaction = null;
            try
            {
                DataSet dsSchema = _mysqlDbCommandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.MySql.XmlModFile;
                ds.ReadXml(xmlFile);

                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                _mysqlDbOperation.Update(ds, _mysqlDbCommandBuilder, mysqlTransaction);
                mysqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        [Test]
        public void TestRefresh()
        {
            TestDeleteAll();
            TestInsert();

            IDbTransaction mysqlTransaction = null;
            try
            {
                DataSet dsSchema = _mysqlDbCommandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.MySql.XmlRefreshFile;
                ds.ReadXml(xmlFile);

                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                _mysqlDbOperation.Refresh(ds, _mysqlDbCommandBuilder, mysqlTransaction);
                mysqlTransaction.Commit();

            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Commit();
//                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

        private void resetIdentityColumns()
        {
            IDbTransaction mysqlTransaction = null;
            try
            {
                DataSet dsSchema = _mysqlDbCommandBuilder.GetSchema();
                mysqlTransaction = _mysqlDbCommandBuilder.Connection.BeginTransaction();
                foreach (DataTable table in dsSchema.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.AutoIncrement)
                        {
                            //String mysql = "dbcc checkident" + table.TableName + ", RESEED, 0)";
                            //MySqlCommand mysqlCommand = new MySqlCommand(mysql, (MySqlConnection)_mysqlDbCommandBuilder.Connection);
                            //mysqlCommand.Transaction = (MySqlTransaction)mysqlTransaction;
                            //mysqlCommand.ExecuteNonQuery();

                            //break;
                        }
                    }
                }
                mysqlTransaction.Commit();
            }
            catch (Exception e)
            {
                if (mysqlTransaction != null)
                {
                    mysqlTransaction.Rollback();
                }

                throw (e);
            }
        }

    }
}
