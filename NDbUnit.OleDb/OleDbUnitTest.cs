/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2010
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

using System.Data;
using System.Data.Common;
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
        /// <summary>
        /// Initializes a new instance of the <see cref="OleDbUnitTest"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string 
        /// used to open the database.
        /// <seealso cref="System.Data.IDbConnection"/></param>

        public OleDbUnitTest(IDbConnection connection)
            : base(connection)
        {
        }

        public OleDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Gets or sets the OLE database type.  The default value for an 
        /// instance of an object is <see cref="OleDb.OleDbType.NoDb" />.
        /// </summary>
        public OleDbType OleOleDbType
        {
            get
            {
                return ((OleDbOperation)GetDbOperation()).OleOleDbType;
            }

            set
            {
                ((OleDbOperation)GetDbOperation()).OleOleDbType = value;
            }
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new OleDbDataAdapter((OleDbCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection)
        {
            return new OleDbCommandBuilder(connection);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
        {
            return new OleDbCommandBuilder(connectionString);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new OleDbOperation();
        }

        protected override void OnGetDataSetFromDb(string tableName, ref DataSet dsToFill, IDbConnection dbConnection)
        {
            OleDbCommand selectCommand = (OleDbCommand)GetDbCommandBuilder().GetSelectCommand(tableName);
            selectCommand.Connection = dbConnection as OleDbConnection;
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCommand);
            adapter.Fill(dsToFill, tableName);
        }

    }
}
