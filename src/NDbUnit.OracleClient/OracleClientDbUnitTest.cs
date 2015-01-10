/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2011
 * http://code.google.com/p/ndbunit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System.Data;
using NDbUnit.Core;
using Oracle.DataAccess.Client;

namespace NDbUnit.OracleClient
{
    public class OracleClientDbUnitTest : NDbUnitTest<OracleConnection>
    {
        public OracleClientDbUnitTest(OracleConnection connection)
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

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<OracleConnection> connectionManager)
        {
            OracleClientDbCommandBuilder commandBuilder = new OracleClientDbCommandBuilder(connectionManager);
            commandBuilder.CommandTimeOutSeconds = this.CommandTimeOut;
            return commandBuilder;
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new OracleClientDbOperation();
        }
    }
}
