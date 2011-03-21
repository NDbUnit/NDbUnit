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

using NDbUnit.Core;
using NDbUnit.Core.MySqlClient;
using NDbUnit.Postgresql;

namespace NDbUnit.Test.Postgresql
{
    public class PostgresqlDbUnitIntegrationTest : IntegationTestBase
    {
        protected override INDbUnitTest GetNDbUnitTest()
        {
            return new PostgresqlDbUnitTest(DbConnection.PostgresqlConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Postgresql.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.Postgresql.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.Postgresql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Postgresql.XmlSchemaFile;
        }
    }
}