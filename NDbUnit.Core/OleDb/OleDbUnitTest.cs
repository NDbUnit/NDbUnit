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

namespace NDbUnit.Core.OleDb
{
	/// <summary>
	/// The OleDb unit test data adapter.
	/// </summary>
	/// <example>
	/// <code>
	/// string connectionString = "Provider=SQLOLEDB;Data Source=V-AL-DIMEOLA\NETSDK;Initial Catalog=testdb;Integrated Security=SSPI;";
	/// OleDbUnitTest oleDbUnitTest = new OleDbUnitTest(connectionString);
	/// string xmlSchemaFile = "User.xsd";
	/// string xmlFile = "User.xml";
	///	oleDbUnitTest.ReadXmlSchema(xmlSchemaFile);
	///	oleDbUnitTest.ReadXml(xmlFile);
	///	oleDbUnitTest.PerformDbOperation(DbOperation.CleanInsertIdentity);
	/// </code>
	/// <seealso cref="INDbUnitTest"/>
	/// </example>
	public class OleDbUnitTest : NDbUnitTest
	{
		#region Private Fields

		OleDbCommandBuilder _oleDbCommandBuilder = null;
		OleDbOperation _oleDbOperation = null;

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="OleDbUnitTest"/> class.
		/// </summary>
		/// <param name="connectionString">The connection string 
		/// used to open the database.
		/// <seealso cref="System.Data.IDbConnection"/></param>
		public OleDbUnitTest(string connectionString)
		{
			_oleDbCommandBuilder = new OleDbCommandBuilder(connectionString);
			_oleDbOperation = new OleDbOperation();
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets the OLE database type.  The default value for an 
		/// instance of an object is <see cref="DbType.NoDb" />.
		/// </summary>
		public DbType OleDbType
		{
			get
			{
				return _oleDbOperation.OleDbType;
			}

			set
			{
				_oleDbOperation.OleDbType = value;
			}
		}

		#endregion

		#region Protected Overrides

		protected override IDbCommandBuilder GetDbCommandBuilder()
		{
			return _oleDbCommandBuilder;
		}

		protected override IDbOperation GetDbOperation()
		{
			return _oleDbOperation;
		}

		protected override void OnGetDataSetFromDb(string tableName, ref System.Data.DataSet dsToFill, IDbConnection dbConnection)
		{
			OleDbCommand selectCommand = _oleDbCommandBuilder.GetSelectCommand(tableName);
			selectCommand.Connection = dbConnection as OleDbConnection;
			OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
			adapter.Fill(dsToFill, tableName);
		}

		#endregion
	}
}
