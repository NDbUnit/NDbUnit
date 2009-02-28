using System;
using System.Data;
using System.Data.SQLite;

namespace NDbUnit.Core.SqlLite
{
    public class SqlLiteDbOperation : DbOperation
    {

        # region Type Safe Interface Implementation

        public void Insert(DataSet ds, SqlLiteDbCommandBuilder sqlDbCommandBuilder, SQLiteTransaction sqlTransaction)
        {
            ((IDbOperation)this).Insert(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void InsertIdentity(DataSet ds, SqlLiteDbCommandBuilder sqlDbCommandBuilder, SQLiteTransaction sqlTransaction)
        {
            ((IDbOperation)this).InsertIdentity(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void Update(DataSet ds, SqlLiteDbCommandBuilder sqlDbCommandBuilder, SQLiteTransaction sqlTransaction)
        {
            ((IDbOperation)this).Update(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void Delete(DataSet ds, SqlLiteDbCommandBuilder sqlDbCommandBuilder, SQLiteTransaction sqlTransaction)
        {
            ((IDbOperation)this).Delete(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        public void DeleteAll(SqlLiteDbCommandBuilder sqlDbCommandBuilder, SQLiteTransaction sqlTransaction)
        {
            ((IDbOperation)this).DeleteAll(sqlDbCommandBuilder, sqlTransaction);
        }

        public void Refresh(DataSet ds, SqlLiteDbCommandBuilder sqlDbCommandBuilder, SQLiteTransaction sqlTransaction)
        {
            ((IDbOperation)this).Refresh(ds, sqlDbCommandBuilder, sqlTransaction);
        }

        #endregion

        #region Protected Overrides

        protected override void OnInsert(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SQLiteTransaction sqlTransaction = (SQLiteTransaction)dbTransaction;

            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter();
            sqlDataAdapter.InsertCommand = (SQLiteCommand)dbCommand;
            sqlDataAdapter.InsertCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.InsertCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(dataTable);
        }

        /// <summary>
        /// SQLite doesn't need any changes to insert PK values.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="dbCommand"></param>
        /// <param name="dbTransaction"></param>
        protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            OnInsert(dataTable, dbCommand, dbTransaction);
        }

        protected override void OnDelete(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SQLiteTransaction sqlTransaction = (SQLiteTransaction)dbTransaction;

            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter();
            sqlDataAdapter.DeleteCommand = (SQLiteCommand)dbCommand;
            sqlDataAdapter.DeleteCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.DeleteCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(dataTable);
        }

        protected override void OnDeleteAll(IDbCommand dbCommand, IDbTransaction dbTransaction)
        {
            SQLiteTransaction sqlTransaction = (SQLiteTransaction)dbTransaction;

            SQLiteCommand sqlCommand = (SQLiteCommand)dbCommand;
            sqlCommand.Connection = sqlTransaction.Connection;
            sqlCommand.Transaction = sqlTransaction;

            sqlCommand.ExecuteNonQuery();
        }

        protected override void OnUpdate(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName)
        {
            SQLiteTransaction sqlTransaction = (SQLiteTransaction)dbTransaction;

            SQLiteDataAdapter sqlDataAdapter = new SQLiteDataAdapter();
            sqlDataAdapter.UpdateCommand = (SQLiteCommand)dbCommandBuilder.GetUpdateCommand(tableName);
            sqlDataAdapter.UpdateCommand.Connection = sqlTransaction.Connection;
            sqlDataAdapter.UpdateCommand.Transaction = sqlTransaction;

            sqlDataAdapter.Update(ds, tableName);
        }

        protected override void OnRefresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName)
        {
            SQLiteTransaction transaction = (SQLiteTransaction)dbTransaction;

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();
            dataAdapter.SelectCommand = (SQLiteCommand)dbCommandBuilder.GetSelectCommand(tableName);
            dataAdapter.SelectCommand.Connection = transaction.Connection;
            dataAdapter.SelectCommand.Transaction = transaction;

            DataSet dsDb = new DataSet();
            // Query all records in the database table.
            dataAdapter.Fill(dsDb, tableName);

            DataSet dsUpdate = dbCommandBuilder.GetSchema().Clone();

            DataTable refreshTable = ds.Tables[tableName];
            DataTable dataTableDb = dsDb.Tables[tableName];
            // Iterate all rows in the table.
            foreach (DataRow freshDataRow in refreshTable.Rows)
            {
                bool rowDoesNotExist = true;
                // Iterate all rows in the database table.
                foreach (DataRow dataRowDb in dataTableDb.Rows)
                {
                    // The row exists in the database.
                    if (IsPrimaryKeyValueEqual(freshDataRow, dataRowDb, dsUpdate.Tables[tableName].PrimaryKey))
                    {
                        rowDoesNotExist = false;                        
                        DataRow dataRowNew = base.CloneDataRow(dsUpdate.Tables[tableName], freshDataRow);
                        dsUpdate.Tables[tableName].Rows.Add(dataRowNew);
                        dataRowNew.AcceptChanges();

                        MarkRowAsModified(dataRowNew);

                        break;
                    }

                }

                // The row does not exist in the database.
                if (rowDoesNotExist)
                {
                    DataRow dataRowNew = base.CloneDataRow(dsUpdate.Tables[tableName], freshDataRow);
                    dsUpdate.Tables[tableName].Rows.Add(dataRowNew);

                }
            }

            // Does not insert identity.
            dataAdapter.InsertCommand = (SQLiteCommand)dbCommandBuilder.GetInsertCommand(tableName);
            dataAdapter.InsertCommand.Connection = transaction.Connection;
            dataAdapter.InsertCommand.Transaction = transaction;

            dataAdapter.UpdateCommand = (SQLiteCommand)dbCommandBuilder.GetUpdateCommand(tableName);
            dataAdapter.UpdateCommand.Connection = transaction.Connection;
            dataAdapter.UpdateCommand.Transaction = transaction;

            dataAdapter.Update(dsUpdate, tableName);
        }

        private void MarkRowAsModified(DataRow dataRow)
        {
            dataRow.BeginEdit();
            dataRow.EndEdit();
        }

        #endregion
    }
}
