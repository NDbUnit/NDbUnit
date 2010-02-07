using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.IO;
using NDbUnit.Core;
using System.Data;

namespace NDbUnit.Test
{
    [TestFixture]
    public abstract class IntegationTestBase
    {
        [Test]
        public void Delete_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(GetXmlSchemaFilename());

            database.ReadXmlSchema(GetXmlSchemaFilename());
            database.ReadXml(GetXmlFilename());

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);
            database.PerformDbOperation(DbOperationFlag.DeleteAll);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.AreEqual(preOperation, postOperation, new StructuralEqualityComparer<DataSet>());
        }

        [Test]
        public void InsertIdentity_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(GetXmlSchemaFilename());
            preOperation.ReadXml(GetXmlFilename());

            database.ReadXmlSchema(GetXmlSchemaFilename());
            database.ReadXml(GetXmlFilename());

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.AreEqual(preOperation, postOperation, new StructuralEqualityComparer<DataSet>());

        }

        [Test]
        public void Refresh_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(GetXmlSchemaFilename());
            preOperation.ReadXml(GetXmlFilename());

            database.ReadXmlSchema(GetXmlSchemaFilename());
            database.ReadXml(GetXmlFilename());

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            database.ReadXml(GetXmlRefreshFilename());
            database.PerformDbOperation(DbOperationFlag.Refresh);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.AreEqual(preOperation, postOperation, new StructuralEqualityComparer<DataSet>());

        }

        [Test]
        public void Update_Operation_Matches_Expected_Data()
        {
            INDbUnitTest database = GetNDbUnitTest();

            DataSet preOperation = new DataSet();
            preOperation.ReadXmlSchema(GetXmlSchemaFilename());
            preOperation.ReadXml(GetXmlModFilename());

            database.ReadXmlSchema(GetXmlSchemaFilename());
            database.ReadXml(GetXmlFilename());

            database.PerformDbOperation(DbOperationFlag.DeleteAll);
            database.PerformDbOperation(DbOperationFlag.InsertIdentity);

            database.ReadXml(GetXmlModFilename());
            database.PerformDbOperation(DbOperationFlag.Update);

            DataSet postOperation = database.GetDataSetFromDb();

            Assert.AreEqual(preOperation, postOperation, new StructuralEqualityComparer<DataSet>());

        }

        protected abstract INDbUnitTest GetNDbUnitTest();

        protected abstract string GetXmlFilename();

        protected abstract string GetXmlModFilename();

        protected abstract string GetXmlRefreshFilename();

        protected abstract string GetXmlSchemaFilename();

    }
}
