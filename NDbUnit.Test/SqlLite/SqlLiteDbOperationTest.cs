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
using System.Data.SQLite;
using MbUnit.Framework;
using NDbUnit.Core.SqlLite;

namespace NDbUnit.Test.SqlLite
{
    [TestFixture]
    public class SqlLiteDbOperationTest
    {
        private bool _built = false;
        private SqlLiteDbOperation _sqlDbOperation = new SqlLiteDbOperation();
        private SqlLiteDbCommandBuilder _sqlDbCommandBuilder = new SqlLiteDbCommandBuilder(DbConnection.SqlLiteConnectionString);
        private DataSet _dsData = null;

        [SetUp]
        public void SetUp()
        {
            if (false == _built)
            {
                string xmlSchemaFile = XmlTestFiles.xmlSchemaFileForSQLite;
                string xmlFile = XmlTestFiles.XmlFile;

                try
                {
                    _sqlDbCommandBuilder.BuildCommands(xmlSchemaFile);
                }
                catch (Exception e)
                {
                    throw (e);
                }

                _built = true;

                DataSet dsSchema = _sqlDbCommandBuilder.GetSchema();
                _dsData = dsSchema.Clone();
                _dsData.ReadXml(xmlFile);
            }

            _sqlDbCommandBuilder.Connection.Open();
        }

        [TearDown]
        public void TearDown()
        {
            _sqlDbCommandBuilder.Connection.Close();
        }

        [Test]
        public void TestInsert()
        {
            resetIdentityColumns();

            TestDeleteAll();

            IDbTransaction sqlTransaction = null;
            try
            {
                sqlTransaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
                _sqlDbOperation.Insert(_dsData, _sqlDbCommandBuilder, sqlTransaction);
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
                sqlTransaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
                _sqlDbOperation.InsertIdentity(_dsData, _sqlDbCommandBuilder, sqlTransaction);
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
                sqlTransaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
                _sqlDbOperation.DeleteAll(_sqlDbCommandBuilder, sqlTransaction);
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
                sqlTransaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
                _sqlDbOperation.Delete(_dsData, _sqlDbCommandBuilder, sqlTransaction);
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
                DataSet dsSchema = _sqlDbCommandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.XmlModFile;
                ds.ReadXml(xmlFile);

                sqlTransaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
                _sqlDbOperation.Update(ds, _sqlDbCommandBuilder, sqlTransaction);
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
                DataSet dsSchema = _sqlDbCommandBuilder.GetSchema();
                DataSet ds = dsSchema.Clone();
                string xmlFile = XmlTestFiles.XmlRefreshFile;
                ds.ReadXml(xmlFile);

                sqlTransaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
                _sqlDbOperation.Refresh(ds, _sqlDbCommandBuilder, sqlTransaction);
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

		private void resetIdentityColumns()
		{
            IDbTransaction transaction = null;
			try
			{
				DataSet dsSchema = _sqlDbCommandBuilder.GetSchema();
				transaction = _sqlDbCommandBuilder.Connection.BeginTransaction();
				foreach(DataTable table in dsSchema.Tables)
				{
					foreach(DataColumn column in table.Columns)
					{
						if(column.AutoIncrement)
						{
						    String sql = "delete from sqlite_sequence where name = '" + table.TableName + "'"; 
                            Console.Out.WriteLine(sql);
							SQLiteCommand command = new SQLiteCommand(sql, (SQLiteConnection) _sqlDbCommandBuilder.Connection);
							command.Transaction = (SQLiteTransaction) transaction;
							command.ExecuteNonQuery();

							break;
						}
					}				
				}
				transaction.Commit();
			}
			catch(Exception e)
			{
				if (transaction != null)
				{
					transaction.Rollback();
				}

				throw(e);
			}
		}
    }
}
