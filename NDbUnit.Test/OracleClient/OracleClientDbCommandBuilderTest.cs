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
using NDbUnit.Test.Common;
using NDbUnit.OracleClient;
using NDbUnit.Core;
using MbUnit.Framework;

namespace NDbUnit.Test.OracleClient
{
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
            return new OracleClientDbCommandBuilder(DbConnection.OracleClientConnectionString);
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

    }
}
