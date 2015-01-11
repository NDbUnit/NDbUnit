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

using System;
using System.Collections.Generic;
using System.Text;
using NDbUnit.Test.Common;
using NDbUnit.OracleClient;
using NDbUnit.Core;
using System.Data;
using System.IO;
using NUnit.Framework;
using Oracle.DataAccess.Client;

namespace NDbUnit.Test.OracleClient
{
    [Category(TestCategories.OracleTests)]
    public class OracleClientDbUnitTestTest : DbUnitTestTestBase
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

        protected override IUnitTestStub GetUnitTestStub()
        {
            return new OracleClientDbUnitTestStub(DbConnection.OracleClientConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OracleClient.XmlFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

        protected class OracleClientDbUnitTestStub : OracleClientDbUnitTest, IUnitTestStub
        {
            public OracleClientDbUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<OracleConnection> connectionManager)
            {
                return _mockDbCommandBuilder;
            }

            protected override IDbOperation CreateDbOperation()
            {
                return _mockDbOperation;
            }

            protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
            {
                return base.CreateDataAdapter(command);
            }

            protected override FileStream GetXmlSchemaFileStream(string xmlSchemaFile)
            {
                return _mockSchemaFileStream;
            }

            protected override FileStream GetXmlDataFileStream(string xmlFile)
            {
                return _mockDataFileStream;
            }

            protected override DataSet DS
            {
                get { return base.DS; }
            }

            public DataSet TestDataSet
            {
                get { return DS; }
            }
        }
    }
}
