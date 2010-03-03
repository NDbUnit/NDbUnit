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

using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace NDbUnit.Core.SqlClient
{
    /// <summary>
    /// The Sql Server unit test data adapter.
    /// </summary>
    /// <example>
    /// <code>
    /// string connectionString = "Persist Security Info=False;Integrated Security=SSPI;database=testdb;server=V-AL-DIMEOLA\NETSDK";
    /// SqlDbUnitTest sqlDbUnitTest = new SqlDbUnitTest(connectionString);
    /// string xmlSchemaFile = "User.xsd";
    /// string xmlFile = "User.xml";
    /// sqlDbUnitTest.ReadXmlSchema(xmlSchemaFile);
    /// sqlDbUnitTest.ReadXml(xmlFile);
    /// sqlDbUnitTest.PerformDbOperation(DbOperation.CleanInsertIdentity);
    /// </code>
    /// <seealso cref="INDbUnitTest"/>
    /// </example>
    public class SqlDbUnitTest : NDbUnitTest
    {
        public SqlDbUnitTest(IDbConnection connection)
            : base(connection)
        {
        }

        public SqlDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
        {
            SqlDbCommandBuilder commandBuilder = new SqlDbCommandBuilder(connectionString);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection)
        {
            SqlDbCommandBuilder commandBuilder = new SqlDbCommandBuilder(connection);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new SqlDbOperation();
        }

    }
}
