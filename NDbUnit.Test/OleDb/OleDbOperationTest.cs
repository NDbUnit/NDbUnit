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
using System.Data.OleDb;
using NDbUnit.Core.OleDb;
using MbUnit.Framework;

namespace NDbUnit.Test.OleDb
{
    [TestFixture]
    public class OleDbOperationTest
    {
        private OleDbOperation _oleDbOperation;

        private Core.OleDb.OleDbCommandBuilder _oleDbCommandBuilder;

        private DataSet _dsData;

        [FixtureSetUp]
        public void FixtureSetUp()
        {
            _oleDbCommandBuilder =
                new Core.OleDb.OleDbCommandBuilder(DbConnection.OleDbConnectionString);
            _oleDbOperation = new OleDbOperation();

				string xmlSchemaFile = XmlTestFiles.OleDb.XmlSchemaFile;
				string xmlFile = XmlTestFiles.OleDb.XmlFile;

				try
				{
					_oleDbCommandBuilder.BuildCommands(xmlSchemaFile);
				}
				catch(Exception e)
				{
					throw(e);
				}

            DataSet dsSchema = _oleDbCommandBuilder.GetSchema();
            _dsData = dsSchema.Clone();
            _dsData.ReadXml(xmlFile);
        }

        [SetUp]
        public void SetUp()
        {
            _oleDbOperation.OleOleDbType = Core.OleDb.OleDbType.SqlServer;
            _oleDbCommandBuilder.Connection.Open();
        }

		[TearDown]
		public void TearDown()
		{
			_oleDbCommandBuilder.Connection.Close();
		}

		[Test]
		public void TestInsert()
		{
			resetIdentityColumns();

			TestDeleteAll();

			OleDbTransaction oleDbTransaction = null;
			try
			{
				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();
				_oleDbOperation.Insert(_dsData, _oleDbCommandBuilder, oleDbTransaction);
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}

        [Test]
        public void TestInsertIdentity()
        {
            //bad, bad
            TestDeleteAll();

			OleDbTransaction oleDbTransaction = null;
			try
			{
				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();
				_oleDbOperation.InsertIdentity(_dsData, _oleDbCommandBuilder, oleDbTransaction);
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}

		[Test]
		public void TestDeleteAll()
		{
			OleDbTransaction oleDbTransaction = null;
			try
			{
				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();
				_oleDbOperation.DeleteAll(_oleDbCommandBuilder, oleDbTransaction);
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}

		[Test]
		public void TestDelete()
		{
			OleDbTransaction oleDbTransaction = null;
			try
			{
				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();
				_oleDbOperation.Delete(_dsData, _oleDbCommandBuilder, oleDbTransaction);
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}

		[Test]
		public void TestUpdate()
		{
			TestDeleteAll();
			TestInsert();

			OleDbTransaction oleDbTransaction = null;
			try
			{
				DataSet dsSchema = _oleDbCommandBuilder.GetSchema();
				DataSet ds = dsSchema.Clone();
				string xmlFile = XmlTestFiles.OleDb.XmlModFile;
				ds.ReadXml(xmlFile);

				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();
				_oleDbOperation.Update(ds, _oleDbCommandBuilder, oleDbTransaction);
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}

		[Test]
		public void TestRefresh()
		{
			TestDeleteAll();
			TestInsert();

			OleDbTransaction oleDbTransaction = null;
			try
			{
				DataSet dsSchema = _oleDbCommandBuilder.GetSchema();
				DataSet ds = dsSchema.Clone();
				string xmlFile = XmlTestFiles.OleDb.XmlRefreshFile;
				ds.ReadXml(xmlFile);

				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();
				_oleDbOperation.Refresh(ds, _oleDbCommandBuilder, oleDbTransaction);
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}

		private void resetIdentityColumns()
		{
			OleDbTransaction oleDbTransaction = null;
			try
			{
				DataSet dsSchema = _oleDbCommandBuilder.GetSchema();
				oleDbTransaction = _oleDbCommandBuilder.Connection.BeginTransaction();

                foreach (DataTable table in dsSchema.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        if (column.AutoIncrement)
                        {
                            String sql = "dbcc checkident([" + table.TableName + "], RESEED, 0)";
                            OleDbCommand oleDbCommand = new OleDbCommand(sql, _oleDbCommandBuilder.Connection);
                            oleDbCommand.Transaction = oleDbTransaction;
                            oleDbCommand.ExecuteNonQuery();

							break;
						}
					}
				}
				oleDbTransaction.Commit();
			}
			catch(Exception e)
			{
				if (oleDbTransaction != null)
				{
					oleDbTransaction.Rollback();
				}

				throw(e);
			}
		}
	}
}
