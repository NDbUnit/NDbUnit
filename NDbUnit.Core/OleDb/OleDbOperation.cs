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
using System.Collections;

namespace NDbUnit.Core.OleDb
{
	/// <summary>
	/// An enumeration specifying the OLE database type.
	/// </summary>
	public enum DbType
	{
		/// <summary>No database.</summary>
		NoDb, 
		/// <summary>Sql server.</summary>
		SqlServer, 
		/// <summary>Oracle.</summary>
		Oracle,
		/// <summary>DB2.</summary>
		DB2
	}

	public class OleDbOperation : DbOperation
	{
		#region Private Fields

		private DbType _oleDbType = DbType.NoDb;

		#endregion

		#region Public Methods

		public OleDbOperation()
		{
            QuotePrefix = "[";
            QuoteSuffix = "]";
		}

		#endregion

		#region Public Properties

		public DbType OleDbType
		{
			get
			{
				return _oleDbType;
			}

			set
			{
				_oleDbType = value;
			}
		}
	
		#endregion

		# region Type Safe Interface Implementation

		public void Insert(DataSet ds, OleDbCommandBuilder oleDbDbCommandBuilder, OleDbTransaction oleDbTransaction)
		{
			((IDbOperation)this).Insert(ds, oleDbDbCommandBuilder, oleDbTransaction);
		}

		public void InsertIdentity(DataSet ds, OleDbCommandBuilder oleDbDbCommandBuilder, OleDbTransaction oleDbTransaction)
		{
			((IDbOperation)this).InsertIdentity(ds, oleDbDbCommandBuilder, oleDbTransaction);
		}

		public void Update(DataSet ds, OleDbCommandBuilder oleDbDbCommandBuilder, OleDbTransaction oleDbTransaction)
		{
			((IDbOperation)this).Update(ds, oleDbDbCommandBuilder, oleDbTransaction);
		}

		public void Delete(DataSet ds, OleDbCommandBuilder oleDbDbCommandBuilder, OleDbTransaction oleDbTransaction)
		{
			((IDbOperation)this).Delete(ds, oleDbDbCommandBuilder, oleDbTransaction);
		}

		public void DeleteAll(OleDbCommandBuilder oleDbDbCommandBuilder, OleDbTransaction oleDbTransaction)
		{
			((IDbOperation)this).DeleteAll(oleDbDbCommandBuilder, oleDbTransaction);
		}

		public void Refresh(DataSet ds, OleDbCommandBuilder oleDbDbCommandBuilder, OleDbTransaction oleDbTransaction)
		{
			((IDbOperation)this).Refresh(ds, oleDbDbCommandBuilder, oleDbTransaction);
		}

		#endregion

		#region Protected Overrides

		protected override void OnInsert(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
		{
			OleDbTransaction oleDbTransaction = (OleDbTransaction)dbTransaction;

			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.InsertCommand = (OleDbCommand)dbCommand;
			oleDbDataAdapter.InsertCommand.Connection = oleDbTransaction.Connection;
			oleDbDataAdapter.InsertCommand.Transaction = oleDbTransaction;

			oleDbDataAdapter.Update(dataTable);
		}

		protected override void OnInsertIdentity(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
		{
			OleDbTransaction oleDbTransaction = (OleDbTransaction)dbTransaction;

			if (_oleDbType == DbType.SqlServer)
			{
				foreach(DataColumn column in dataTable.Columns)
				{
					if(column.AutoIncrement)
					{
						// Set identity insert on.
						OleDbCommand oleDbCommand = new OleDbCommand("SET IDENTITY_INSERT " + TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) + " ON");
						oleDbCommand.Connection = oleDbTransaction.Connection;
						oleDbCommand.Transaction = oleDbTransaction;
						oleDbCommand.ExecuteNonQuery();

						break;
					}
				}
			}

			try
			{
				OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
				oleDbDataAdapter.InsertCommand = (OleDbCommand)dbCommand;
				oleDbDataAdapter.InsertCommand.Connection = oleDbTransaction.Connection;
				oleDbDataAdapter.InsertCommand.Transaction = oleDbTransaction;

				oleDbDataAdapter.Update(dataTable);
			}
			catch(Exception e)
			{
				throw(e);
			}
			finally
			{
				if (_oleDbType == DbType.SqlServer)
				{
					foreach(DataColumn column in dataTable.Columns)
					{
						if(column.AutoIncrement)
						{
							// Set identity insert off.
							OleDbCommand oleDbCommand = new OleDbCommand("SET IDENTITY_INSERT " + TableNameHelper.FormatTableName(dataTable.TableName, QuotePrefix, QuoteSuffix) + " OFF");
							oleDbCommand.Connection = oleDbTransaction.Connection;
							oleDbCommand.Transaction = oleDbTransaction;
							oleDbCommand.ExecuteNonQuery();

							break;
						}
					}
				}
			}
		}
		
		protected override void OnDelete(DataTable dataTable, IDbCommand dbCommand, IDbTransaction dbTransaction)
		{
			OleDbTransaction oleDbTransaction = (OleDbTransaction)dbTransaction;

			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.DeleteCommand = (OleDbCommand)dbCommand;
			oleDbDataAdapter.DeleteCommand.Connection = oleDbTransaction.Connection;
			oleDbDataAdapter.DeleteCommand.Transaction = oleDbTransaction;

			oleDbDataAdapter.Update(dataTable);
		}

		protected override void OnDeleteAll(IDbCommand dbCommand, IDbTransaction dbTransaction)
		{
			OleDbTransaction oleDbTransaction = (OleDbTransaction)dbTransaction;

			OleDbCommand oleDbCommand = (OleDbCommand)dbCommand;
			oleDbCommand.Connection = oleDbTransaction.Connection;
			oleDbCommand.Transaction = oleDbTransaction;

			oleDbCommand.ExecuteNonQuery();
		}

		protected override void OnUpdate(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName)
		{
			OleDbTransaction oleDbTransaction = (OleDbTransaction)dbTransaction;

			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.UpdateCommand = (OleDbCommand)dbCommandBuilder.GetUpdateCommand(tableName);
			oleDbDataAdapter.UpdateCommand.Connection = oleDbTransaction.Connection;
			oleDbDataAdapter.UpdateCommand.Transaction = oleDbTransaction;

			oleDbDataAdapter.Update(ds, tableName);
		}

		protected override void OnRefresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, IDbTransaction dbTransaction, string tableName)
		{
			OleDbTransaction oleDbTransaction = (OleDbTransaction)dbTransaction;

			OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
			oleDbDataAdapter.SelectCommand = (OleDbCommand)dbCommandBuilder.GetSelectCommand(tableName); 
			oleDbDataAdapter.SelectCommand.Connection = oleDbTransaction.Connection;
			oleDbDataAdapter.SelectCommand.Transaction = oleDbTransaction;

			DataSet dsDb = new DataSet();
			// Query all records in the database table.
			oleDbDataAdapter.Fill(dsDb, tableName);			

			DataSet dsUpdate = dbCommandBuilder.GetSchema().Clone();

			DataTable dataTable = ds.Tables[tableName];
			DataTable dataTableDb = dsDb.Tables[tableName];
			// Iterate all rows in the table.
			foreach(DataRow dataRow in dataTable.Rows)
			{
				string row = dataRow.ToString();
				bool rowDoesNotExist = true;
				// Iterate all rows in the database table.
				foreach(DataRow dataRowDb in dataTableDb.Rows)
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
			oleDbDataAdapter.InsertCommand = (OleDbCommand)dbCommandBuilder.GetInsertCommand(tableName);
			oleDbDataAdapter.InsertCommand.Connection = oleDbTransaction.Connection;
			oleDbDataAdapter.InsertCommand.Transaction = oleDbTransaction;

			oleDbDataAdapter.UpdateCommand = (OleDbCommand)dbCommandBuilder.GetUpdateCommand(tableName);
			oleDbDataAdapter.UpdateCommand.Connection = oleDbTransaction.Connection;
			oleDbDataAdapter.UpdateCommand.Transaction = oleDbTransaction;

			oleDbDataAdapter.Update(dsUpdate, tableName);
		}

		#endregion
	}
}
