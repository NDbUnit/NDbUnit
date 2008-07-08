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
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace NDbUnit.Core.OleDb
{
	public class OleDbCommandBuilder : DbCommandBuilder
	{
		#region Private Fields

		private OleDbConnection _oleDbConnection = null;
		private DataTable _dataTableSchema = null;

		#endregion

		#region Public Methods

		public OleDbCommandBuilder(string connectionString)
		{
			_oleDbConnection = new OleDbConnection(connectionString);
		}

		#endregion
	
		#region Type Safe Interface Implementation

		public OleDbConnection Connection
		{
			get
			{
				return _oleDbConnection;
			}
		}

		public OleDbCommand GetSelectCommand(string tableName)
		{
			return ((OleDbCommand)((IDbCommandBuilder)this).GetSelectCommand(tableName));
		}

		public OleDbCommand GetInsertCommand(string tableName)
		{
			return ((OleDbCommand)((IDbCommandBuilder)this).GetInsertCommand(tableName));
		}

		public OleDbCommand GetInsertIdentityCommand(string tableName)
		{
			return ((OleDbCommand)((IDbCommandBuilder)this).GetInsertIdentityCommand(tableName));
		}

		public OleDbCommand GetDeleteCommand(string tableName)
		{
			return ((OleDbCommand)((IDbCommandBuilder)this).GetDeleteCommand(tableName));
		}

		public OleDbCommand GetDeleteAllCommand(string tableName)
		{
			return ((OleDbCommand)((IDbCommandBuilder)this).GetDeleteAllCommand(tableName));
		}

		public OleDbCommand GetUpdateCommand(string tableName)
		{
			return ((OleDbCommand)((IDbCommandBuilder)this).GetUpdateCommand(tableName));
		}

		#endregion

		#region Protected Overrides

		protected override IDbConnection GetConnection()
		{
			return _oleDbConnection;
		}

		protected override IDbCommand CreateSelectCommand(DataSet ds, string tableName)
		{
			OleDbCommand oleDbSelectCommand = new OleDbCommand();

			bool notFirstColumn = false;
			StringBuilder sb = new StringBuilder("SELECT ");
			DataTable dataTable = ds.Tables[tableName];
			foreach(DataColumn dataColumn in dataTable.Columns)
			{
				if (notFirstColumn)
				{
					sb.Append(", ");
				}

				notFirstColumn = true;

				sb.Append(base.QuotePrefix + dataColumn.ColumnName + base.QuoteSuffix);
			}

			sb.Append(" FROM ");
			sb.Append(base.QuotePrefix + tableName + base.QuoteSuffix);

			oleDbSelectCommand.CommandText = sb.ToString();
			oleDbSelectCommand.Connection = _oleDbConnection;

            try
            {
                _dataTableSchema = getSchemaTable(oleDbSelectCommand);
            }
		    catch (Exception e)
		    {
                string message = String.Format("OleDbCommandBuilder.CreateSelectCommand(DataSet, string) failed for tableName = '{0}'", tableName);
                throw new NDbUnitException(message, e);
			}

			return oleDbSelectCommand;
		}

		protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
		{	
			OleDbCommand oleDbSelectCommand = (OleDbCommand)selectCommand;

			int count = 1;
			bool notFirstColumn = false;
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO " + base.QuotePrefix + tableName + base.QuoteSuffix + "(");
			StringBuilder sbParam = new StringBuilder();
			OleDbParameter oleDbParameter = null;
			OleDbCommand oleDbInsertCommand = new OleDbCommand();
			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{			
				// Not an identity column.
				if (!((bool)dataRow["IsAutoIncrement"]))
				{
					if (notFirstColumn)
					{
						sb.Append(", ");
						sbParam.Append(", ");
					}

					notFirstColumn = true;

					sb.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
					sbParam.Append("?");

					oleDbParameter = newOleDbParameter(count, dataRow);
					oleDbInsertCommand.Parameters.Add(oleDbParameter);

					++count;
				}
			}			

			sb.Append(") VALUES(" + sbParam.ToString() + ")");

			oleDbInsertCommand.CommandText = sb.ToString();

			return oleDbInsertCommand;
		}

		protected override IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName)
		{
			OleDbCommand oleDbSelectCommand = (OleDbCommand)selectCommand;

			int count = 1;
			bool notFirstColumn = false;
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO " + base.QuotePrefix + tableName + base.QuoteSuffix + "(");
			StringBuilder sbParam = new StringBuilder();
			OleDbParameter oleDbParameter = null;
			OleDbCommand oleDbInsertIdentityCommand = new OleDbCommand();
			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{			
				if (notFirstColumn)
				{
					sb.Append(", ");
					sbParam.Append(", ");
				}

				notFirstColumn = true;

				sb.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
				sbParam.Append("?");

				oleDbParameter = newOleDbParameter(count, dataRow);
				oleDbInsertIdentityCommand.Parameters.Add(oleDbParameter);

				++count;
			}			

			sb.Append(") VALUES(" + sbParam.ToString() + ")");

			oleDbInsertIdentityCommand.CommandText = sb.ToString();

			return oleDbInsertIdentityCommand;
		}

		protected override IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
		{
			OleDbCommand oleDbSelectCommand = (OleDbCommand)selectCommand;

			StringBuilder sb = new StringBuilder();
			sb.Append("DELETE FROM " + base.QuotePrefix + tableName + base.QuoteSuffix + " WHERE ");

			OleDbCommand oleDbDeleteCommand = new OleDbCommand();

			int count = 1;
			OleDbParameter oleDbParameter = null;
			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{			
				// A key column.
				if ((bool)dataRow["IsKey"])
				{
					if (count != 1)
					{
						sb.Append(" AND ");
					}

					sb.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
					sb.Append("=?");

					oleDbParameter = newOleDbParameter(count, dataRow);
					oleDbDeleteCommand.Parameters.Add(oleDbParameter);

					++count;
				}
			}			

			oleDbDeleteCommand.CommandText = sb.ToString();

			return oleDbDeleteCommand;
		}

		protected override IDbCommand CreateDeleteAllCommand(string tableName)
		{
			return new OleDbCommand("DELETE FROM " + base.QuotePrefix + tableName + base.QuoteSuffix);
		}

		protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
		{
			OleDbCommand oleDbSelectCommand = (OleDbCommand)selectCommand;

			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE " + base.QuotePrefix + tableName + base.QuoteSuffix + " SET ");

			OleDbCommand oleDbUpdateCommand = new OleDbCommand();

			int count = 1;
			bool notFirstKey = false;
			bool notFirstColumn = false;
			OleDbParameter oleDbParameter = null;
			StringBuilder sbPrimaryKey = new StringBuilder();
			ArrayList keyParameters = new ArrayList();

			bool containsAllPrimaryKeys = true;
			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{
				if(!(bool)dataRow["IsKey"])
				{
					containsAllPrimaryKeys = false;
					break;
				}
			}

			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{			
				// A key column.
				if ((bool)dataRow["IsKey"])
				{
					if (notFirstKey)
					{
						sbPrimaryKey.Append(" AND ");
					}

					notFirstKey = true;

					sbPrimaryKey.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
					sbPrimaryKey.Append("=?");

					oleDbParameter = newOleDbParameter(count, dataRow);
					keyParameters.Add(oleDbParameter);

					++count;
				}

				if (containsAllPrimaryKeys || !(bool)dataRow["IsKey"])
				{
					if (notFirstColumn)
					{
						sb.Append(", ");
					}

					notFirstColumn = true;

					sb.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
					sb.Append("=?");

					oleDbParameter = newOleDbParameter(count, dataRow);
					oleDbUpdateCommand.Parameters.Add(oleDbParameter);

					++count;
				}
			}			

			// Add key parameters last since ordering is important.
			for (int i = 0; i < keyParameters.Count; ++i)
			{
				oleDbUpdateCommand.Parameters.Add((OleDbParameter)keyParameters[i]);
			}

			sb.Append(" WHERE " + sbPrimaryKey.ToString());

			oleDbUpdateCommand.CommandText = sb.ToString();

			return oleDbUpdateCommand;
		}

		#endregion

		#region Private Methods

		private DataTable getSchemaTable(OleDbCommand oleDbSelectCommand)
		{
			DataTable dataTableSchema = null;
			bool isClosed = ConnectionState.Closed == _oleDbConnection.State;

			try
			{
				if (isClosed)
				{
					_oleDbConnection.Open();
				}

				OleDbDataReader oleDbDataReader = oleDbSelectCommand.ExecuteReader(CommandBehavior.KeyInfo);
				dataTableSchema = oleDbDataReader.GetSchemaTable();
				oleDbDataReader.Close();
			}
			finally
			{
				if (isClosed)
				{
					_oleDbConnection.Close();
				}
			}

			return dataTableSchema;
		}

		private OleDbParameter newOleDbParameter(int index, DataRow dataRow)
		{
			return new OleDbParameter("@p" + index.ToString(), (OleDbType)dataRow["ProviderType"], (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
		}

		#endregion

	}
}
