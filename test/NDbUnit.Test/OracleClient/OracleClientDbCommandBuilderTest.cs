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

using System.Collections.Generic;
using NDbUnit.Test.Common;
using NDbUnit.OracleClient;
using NDbUnit.Core;
using NUnit.Framework;
using Oracle.DataAccess.Client;

namespace NDbUnit.Test.OracleClient
{
    [Category(TestCategories.OracleTests)]
    public class OracleClientDbCommandBuilderTest : DbCommandBuilderTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "USER", "USERROLE", "ROLE"
                };
            }
        }

        public override IList<string> ExpectedDeleteAllCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM \"USER\"",
                    "DELETE FROM \"USERROLE\"", 
                    "DELETE FROM \"ROLE\""
                };
            }
        }

        public override IList<string> ExpectedDeleteCommands
        {
            get
            {
                return new List<string>()
                {
                    "DELETE FROM \"USER\" WHERE \"ID\"=:p1",
                    "DELETE FROM \"USERROLE\" WHERE \"USERID\"=:p1 AND \"ROLEID\"=:p2",
                    "DELETE FROM \"ROLE\" WHERE \"ID\"=:p1"
                };
            }
        }

        public override IList<string> ExpectedInsertCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO \"USER\"(\"ID\", \"FIRSTNAME\", \"LASTNAME\", \"AGE\", \"SUPERVISORID\") VALUES(:p1, :p2, :p3, :p4, :p5)", 
                    "INSERT INTO \"USERROLE\"(\"USERID\", \"ROLEID\") VALUES(:p1, :p2)", 
                    "INSERT INTO \"ROLE\"(\"ID\", \"NAME\", \"DESCRIPTION\") VALUES(:p1, :p2, :p3)"
                };

            }
        }

        public override IList<string> ExpectedInsertIdentityCommands
        {
            get
            {
                return new List<string>()
                {
                    "INSERT INTO \"USER\"(\"ID\", \"FIRSTNAME\", \"LASTNAME\", \"AGE\", \"SUPERVISORID\") VALUES(:p1, :p2, :p3, :p4, :p5)", 
                    "INSERT INTO \"USERROLE\"(\"USERID\", \"ROLEID\") VALUES(:p1, :p2)", 
                    "INSERT INTO \"ROLE\"(\"ID\", \"NAME\", \"DESCRIPTION\") VALUES(:p1, :p2, :p3)"
                };
            }
        }

        public override IList<string> ExpectedSelectCommands
        {
            get
            {
                return new List<string>()
                {
                    "SELECT \"ID\", \"FIRSTNAME\", \"LASTNAME\", \"AGE\", \"SUPERVISORID\" FROM \"USER\"", 
                    "SELECT \"USERID\", \"ROLEID\" FROM \"USERROLE\"", 
                    "SELECT \"ID\", \"NAME\", \"DESCRIPTION\" FROM \"ROLE\""
                };
            }
        }

        public override IList<string> ExpectedUpdateCommands
        {
            get
            {
                return new List<string>()
                { 
                    "UPDATE \"USER\" SET \"FIRSTNAME\"=:p2, \"LASTNAME\"=:p3, \"AGE\"=:p4, \"SUPERVISORID\"=:p5 WHERE \"ID\"=:p1",
                    "UPDATE \"USERROLE\" SET \"USERID\"=:p2, \"ROLEID\"=:p4 WHERE \"USERID\"=:p1 AND \"ROLEID\"=:p3",
                    "UPDATE \"ROLE\" SET \"NAME\"=:p2, \"DESCRIPTION\"=:p3 WHERE \"ID\"=:p1"
                };
            }
        }

        protected override IDbCommandBuilder GetDbCommandBuilder()
        {
            return new OracleClientDbCommandBuilder(new DbConnectionManager<OracleConnection>(DbConnection.OracleClientConnectionString));
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

    }
}
