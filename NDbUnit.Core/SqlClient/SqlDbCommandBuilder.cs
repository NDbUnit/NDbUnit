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
using System.Data.SqlClient;

namespace NDbUnit.Core.SqlClient
{
	public class SqlDbCommandBuilder : DbCommandBuilder
	{
		#region Private Fields

		private SqlConnection _sqlConnection = null;
		private DataTable _dataTableSchema = null;

		#endregion

		#region Public Methods

		public SqlDbCommandBuilder(string connectionString)
		{
			_sqlConnection = new SqlConnection(connectionString);

			base.QuotePrefix = "[";
			base.QuoteSuffix = "]";
		}

		#endregion

		#region Type Safe Interface Implementation

		public SqlConnection Connection
		{
			get
			{
				return _sqlConnection;
			}
		}

		public SqlCommand GetSelectCommand(string tableName)
		{
			return ((SqlCommand)((IDbCommandBuilder)this).GetSelectCommand(tableName));
		}

		public SqlCommand GetInsertCommand(string tableName)
		{
			return ((SqlCommand)((IDbCommandBuilder)this).GetInsertCommand(tableName));
		}

		public SqlCommand GetInsertIdentityCommand(string tableName)
		{
			return ((SqlCommand)((IDbCommandBuilder)this).GetInsertIdentityCommand(tableName));
		}

		public SqlCommand GetDeleteCommand(string tableName)
		{
			return ((SqlCommand)((IDbCommandBuilder)this).GetDeleteCommand(tableName));
		}

		public SqlCommand GetDeleteAllCommand(string tableName)
		{
			return ((SqlCommand)((IDbCommandBuilder)this).GetDeleteAllCommand(tableName));
		}

		public SqlCommand GetUpdateCommand(string tableName)
		{
			return ((SqlCommand)((IDbCommandBuilder)this).GetUpdateCommand(tableName));
		}

		#endregion

		#region Protected Overrides

		protected override IDbConnection GetConnection()
		{
			return _sqlConnection;
		}

		protected override IDbCommand CreateSelectCommand(DataSet ds, string tableName)
		{
			SqlCommand sqlSelectCommand = new SqlCommand();

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

			sqlSelectCommand.CommandText = sb.ToString();
			sqlSelectCommand.Connection = _sqlConnection;

            try
            {
                _dataTableSchema = getSchemaTable(sqlSelectCommand);
            }
		    catch (Exception e)
			{
				string message = String.Format("SqlDbCommandBuilder.CreateSelectCommand(DataSet, string) failed for tableName = '{0}'", tableName);
				throw new NDbUnitException(message, e);
			}

			return sqlSelectCommand;
		}

		protected override IDbCommand CreateInsertCommand(IDbCommand selectCommand, string tableName)
		{	
			SqlCommand sqlSelectCommand = (SqlCommand)selectCommand;

			int count = 1;
			bool notFirstColumn = false;
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO " + base.QuotePrefix + tableName + base.QuoteSuffix + "(");
			StringBuilder sbParam = new StringBuilder();
			SqlParameter sqlParameter = null;
			SqlCommand sqlInsertCommand = new SqlCommand();
			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{			
				// Not an identity column.
				if (!((bool)dataRow["IsIdentity"]))
				{
					if (notFirstColumn)
					{
						sb.Append(", ");
						sbParam.Append(", ");
					}

					notFirstColumn = true;

					sb.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
					sbParam.Append("@p" + count.ToString());

					sqlParameter = newSqlParameter(count, dataRow);
					sqlInsertCommand.Parameters.Add(sqlParameter);

					++count;
				}
			}			

			sb.Append(") VALUES(" + sbParam.ToString() + ")");

			sqlInsertCommand.CommandText = sb.ToString();

			return sqlInsertCommand;
		}

		protected override IDbCommand CreateInsertIdentityCommand(IDbCommand selectCommand, string tableName)
		{
			SqlCommand sqlSelectCommand = (SqlCommand)selectCommand;

			int count = 1;
			bool notFirstColumn = false;
			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO " + base.QuotePrefix + tableName + base.QuoteSuffix + "(");
			StringBuilder sbParam = new StringBuilder();
			SqlParameter sqlParameter = null;
			SqlCommand sqlInsertIdentityCommand = new SqlCommand();
			foreach(DataRow dataRow in _dataTableSchema.Rows)
			{			
				if (notFirstColumn)
				{
					sb.Append(", ");
					sbParam.Append(", ");
				}

				notFirstColumn = true;

				sb.Append(base.QuotePrefix + dataRow["ColumnName"] + base.QuoteSuffix);
				sbParam.Append("@p" + count.ToString());

				sqlParameter = newSqlParameter(count, dataRow);
				sqlInsertIdentityCommand.Parameters.Add(sqlParameter);

				++count;
			}			

			sb.Append(") VALUES(" + sbParam.ToString() + ")");

			sqlInsertIdentityCommand.CommandText = sb.ToString();

			return sqlInsertIdentityCommand;
		}

		protected override IDbCommand CreateDeleteCommand(IDbCommand selectCommand, string tableName)
		{
			SqlCommand sqlSelectCommand = (SqlCommand)selectCommand;

			StringBuilder sb = new StringBuilder();
			sb.Append("DELETE FROM " + base.QuotePrefix + tableName + base.QuoteSuffix + " WHERE ");

			SqlCommand sqlDeleteCommand = new SqlCommand();

			int count = 1;
			SqlParameter sqlParameter = null;
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
					sb.Append("=@p" + count.ToString());

					sqlParameter = newSqlParameter(count, dataRow);
					sqlDeleteCommand.Parameters.Add(sqlParameter);

					++count;
				}
			}			

			sqlDeleteCommand.CommandText = sb.ToString();

			return sqlDeleteCommand;
		}

		protected override IDbCommand CreateDeleteAllCommand(string tableName)
		{
			return new SqlCommand("DELETE FROM " + base.QuotePrefix + tableName + base.QuoteSuffix);
		}

		protected override IDbCommand CreateUpdateCommand(IDbCommand selectCommand, string tableName)
		{
			SqlCommand sqlSelectCommand = (SqlCommand)selectCommand;

			StringBuilder sb = new StringBuilder();
			sb.Append("UPDATE " + base.QuotePrefix + tableName + base.QuoteSuffix + " SET ");

			SqlCommand sqlUpdateCommand = new SqlCommand();

			int count = 1;
			bool notFirstKey = false;
			bool notFirstColumn = false;
			SqlParameter sqlParameter = null;
			StringBuilder sbPrimaryKey = new StringBuilder();

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
					sbPrimaryKey.Append("=@p" + count.ToString());

					sqlParameter = newSqlParameter(count, dataRow);
					sqlUpdateCommand.Parameters.Add(sqlParameter);

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
					sb.Append("=@p" + count.ToString());

					sqlParameter = newSqlParameter(count, dataRow);
					sqlUpdateCommand.Parameters.Add(sqlParameter);

					++count;
				}
			}			

			sb.Append(" WHERE " + sbPrimaryKey.ToString());

			sqlUpdateCommand.CommandText = sb.ToString();

			return sqlUpdateCommand;
		}

		#endregion

		#region Private Methods

		private DataTable getSchemaTable(SqlCommand sqlSelectCommand)
		{
			DataTable dataTableSchema = null;
			bool isClosed = ConnectionState.Closed == _sqlConnection.State;

			try
			{
				if (isClosed)
				{
					_sqlConnection.Open();
				}

				SqlDataReader sqlDataReader = sqlSelectCommand.ExecuteReader(CommandBehavior.KeyInfo);
				dataTableSchema = sqlDataReader.GetSchemaTable();
				sqlDataReader.Close();
			}
			finally
			{
				if (isClosed)
				{
					_sqlConnection.Close();
				}
			}

			return dataTableSchema;
		}

		private SqlParameter newSqlParameter(int index, DataRow dataRow)
		{
			return new SqlParameter("@p" + index.ToString(), (SqlDbType)dataRow["ProviderType"], (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
		}

		#endregion
	}
}
