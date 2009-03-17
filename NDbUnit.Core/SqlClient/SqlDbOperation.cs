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
using System.Data.SqlClient;
using System.Collections;

namespace NDbUnit.Core.SqlClient
{
    /// <summary>
    /// Summary description for SqlDbOperation.
    /// </summary>
    public class SqlDbOperation : DbOperation
    {

        public SqlDbOperation()
        {
            base.QuotePrefix = "[";
            base.QuoteSuffix = "]";
        }

        public void DeleteAll(SqlDbCommandBuilder sqlDbCommandBuilder, SqlTransaction sqlTransaction)
        {
            ((IDbOperation)this).DeleteAll(sqlDbCommandBuilder, sqlTransaction);
        }

        public void Delete(DataSet ds, SqlDbCommandBuilder sqlDbCommandBuilder, SqlTransaction sqlTransaction)
        {
            ((IDbOperation)this).Delete(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void Insert(DataSet ds, SqlDbCommandBuilder sqlDbCommandBuilder, SqlTransaction sqlTransaction)
        {
            ((IDbOperation)this).Insert(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void InsertIdentity(DataSet ds, SqlDbCommandBuilder sqlDbCommandBuilder, SqlTransaction sqlTransaction)
        {
            ((IDbOperation)this).InsertIdentity(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void Refresh(DataSet ds, SqlDbCommandBuilder sqlDbCommandBuilder, SqlTransaction sqlTransaction)
        {
            ((IDbOperation)this).Refresh(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void Update(DataSet ds, SqlDbCommandBuilder sqlDbCommandBuilder, SqlTransaction sqlTransaction)
        {
            ((IDbOperation)this).Update(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        protected override void OnDelete(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)dbTransaction;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.DeleteCommand = (SqlCommand)dbCommand;
            sqlDataAdapter.DeleteCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.DeleteCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(dataTable);
        }

        protected override void OnDeleteAll(IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)dbTransaction;

            SqlCommand sqlCommand = (SqlCommand)dbCommand;
            sqlCommand.Connection = sqlTransaction.Connection;
            sqlCommand.Transaction = sqlTransaction;

            sqlCommand.ExecuteNonQuery();
        }

        protected override void OnInsert(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)dbTransaction;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.InsertCommand = (SqlCommand)dbCommand;
            sqlDataAdapter.InsertCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.InsertCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(dataTable);
        }

        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)dbTransaction;

            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.AutoIncrement)
                {
                    // Set identity insert on.
                    SqlCommand sqlCommand = new SqlCommand("SET IDENTITY_INSERT " + TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) + " ON");
                    sqlCommand.Connection = sqlTransaction.Connection;
                    sqlCommand.Transaction = sqlTransaction;
                    sqlCommand.ExecuteNonQuery();

                    break;
                }
            }

            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
                sqlDataAdapter.InsertCommand = (SqlCommand)dbCommand;
                sqlDataAdapter.InsertCommand.Connection = sqlTransaction.Connection;
                sqlDataAdapter.InsertCommand.Transaction = sqlTransaction;

                sqlDataAdapter.Update(dataTable);
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.AutoIncrement)
                    {
                        // Set identity insert off.
                        SqlCommand sqlCommand = new SqlCommand("SET IDENTITY_INSERT " + TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) + " OFF");
                        sqlCommand.Connection = sqlTransaction.Connection;
                        sqlCommand.Transaction = sqlTransaction;
                        sqlCommand.ExecuteNonQuery();

                        break;
                    }
                }
            }
        }

        protected override void OnRefresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)dbTransaction;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.SelectCommand = (SqlCommand)dbCommandBuilder.GetSelectCommand(tableName);
            sqlDataAdapter.SelectCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.SelectCommand.Transaction = sqlTransaction;

            DataSet dsDb = new DataSet();
            // Query all records in the database table.
            sqlDataAdapter.Fill(dsDb, tableName);

            DataSet dsUpdate = dbCommandBuilder.GetSchema().Clone();

            DataTable dataTable = ds.Tables[tableName];
            DataTable dataTableDb = dsDb.Tables[tableName];
            // Iterate all rows in the table.
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string row = dataRow.ToString();
                bool rowDoesNotExist = true;
                // Iterate all rows in the database table.
                foreach (DataRow dataRowDb in dataTableDb.Rows)
                {
                    // The row exists in the database.
                    if (IsPrimaryKeyValueEqual(dataRow, dataRowDb, dsUpdate.Tables[tableName].PrimaryKey))
                    {
                        rowDoesNotExist = false;
                        DataRow dataRowNew = base.CloneDataRow(dsUpdate.Tables[tableName], dataRow);
                        dsUpdate.Tables[tableName].Rows.Add(dataRowNew);
                        dataRowNew.AcceptChanges();
                        // Mark as modified.
                        dataRowNew.BeginEdit();
                        dataRowNew.EndEdit();
                        break;
                    }
                }

                // The row does not exist in the database.
                if (rowDoesNotExist)
                {
                    DataRow dataRowNew = base.CloneDataRow(dsUpdate.Tables[tableName], dataRow);
                    dsUpdate.Tables[tableName].Rows.Add(dataRowNew);
                }
            }

            // Does not insert identity.
            sqlDataAdapter.InsertCommand = (SqlCommand)dbCommandBuilder.GetInsertCommand(tableName);
            sqlDataAdapter.InsertCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.InsertCommand.Transaction = sqlTransaction;

            sqlDataAdapter.UpdateCommand = (SqlCommand)dbCommandBuilder.GetUpdateCommand(tableName);
            sqlDataAdapter.UpdateCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.UpdateCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(dsUpdate, tableName);
        }

        protected override void OnUpdate(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName)
        {
            SqlTransaction sqlTransaction = (SqlTransaction)dbTransaction;

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.UpdateCommand = (SqlCommand)dbCommandBuilder.GetUpdateCommand(tableName);
            sqlDataAdapter.UpdateCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.UpdateCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(ds, tableName);
        }

    }
}
