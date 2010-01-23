/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2009
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
using System.Collections.Generic;
using System.Text;
using NDbUnit.Core;
using System.Data.OracleClient;
using System.Data;

namespace NDbUnit.OracleClient
{
    public class OracleClientDbUnitTest : NDbUnitTest
    {
        public OracleClientDbUnitTest(IDbConnection connection)
            : base(connection)
        {
        }

        public OracleClientDbUnitTest(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            OracleDataAdapter oda = new OracleDataAdapter();
            oda.SelectCommand = (OracleCommand)command;
            return oda;
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
        {
            OracleClientDbCommandBuilder commandBuilder = new OracleClientDbCommandBuilder(connectionString);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection)
        {
            OracleClientDbCommandBuilder commandBuilder = new OracleClientDbCommandBuilder(connection);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new OracleClientDbOperation();
        }
    }
}
