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

using System.IO;
using MbUnit.Framework;
using NDbUnit.Core;
using System.Data;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace NDbUnit.Test
{
    [NUnit.Framework.TestFixture]
    public abstract class IntegationTestBase
    {
        [NUnit.Framework.Test]
        public void Delete_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);
            database.PerformDbOperation(DbOperationFlag.DeleteAll);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.That(preOperation, Is.EqualTo(postOperation).Using(new StructuralEqualityComparer<DataSet>()));
        }

        [NUnit.Framework.Test]
        public void InsertIdentity_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            preOperation.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.That(preOperation, Is.EqualTo(postOperation).Using(new StructuralEqualityComparer<DataSet>()));

        }

        [NUnit.Framework.Test]
        public void Refresh_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            preOperation.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            database.ReadXml(GetXmlRefreshFilename());
            database.PerformDbOperation(DbOperationFlag.Refresh);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.That(preOperation, Is.EqualTo(postOperation).Using(new StructuralEqualityComparer<DataSet>()));

        }

        [NUnit.Framework.Test]
        public void Update_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            preOperation.ReadXml(ReadOnlyStreamFromFilename(GetXmlModFilename()));

            database.ReadXmlSchema(ReadOnlyStreamFromFilename(GetXmlSchemaFilename()));
            database.ReadXml(ReadOnlyStreamFromFilename(GetXmlFilename()));

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            database.ReadXml(GetXmlModFilename());
            database.PerformDbOperation(DbOperationFlag.Update);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.That(preOperation, Is.EqualTo(postOperation).Using(new StructuralEqualityComparer<DataSet>()));

        }

        protected abstract INDbUnitTest GetNDbUnitTest();

        protected abstract string GetXmlFilename();

        protected abstract string GetXmlModFilename();

        protected abstract string GetXmlRefreshFilename();

        protected abstract string GetXmlSchemaFilename();

        private FileStream ReadOnlyStreamFromFilename(string filename)
        {
            return new FileStream(filename, FileMode.Open, FileAccess.Read);
        }

    }
}
