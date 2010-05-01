using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class DataSetAppendXmlTest
    {

        [TestFixture]
        public class When_Appending_Additional_Xml_Data_Files_to_the_DataSet
        {
            [Test]
            public void DataSet_Contains_Both_Appended_and_New_Records()
            {
                var db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);
                db.ReadXmlSchema(XmlTestFiles.SqlServer.XmlSchemaFile);
                db.ReadXml(XmlTestFiles.SqlServer.XmlFile);

                db.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);

                var preAppendDataset = db.GetDataSetFromDb();

                Assert.AreEqual(2, preAppendDataset.Tables["dbo.User"].Rows.Count);
                Assert.AreEqual(2, preAppendDataset.Tables["Role"].Rows.Count);

                db.AppendXml(XmlTestFiles.SqlServer.XmlApppendFile);

                db.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);

                var postAppendDataset = db.GetDataSetFromDb();

                Assert.AreEqual(4, postAppendDataset.Tables["dbo.User"].Rows.Count);
                Assert.AreEqual(4, postAppendDataset.Tables["Role"].Rows.Count);

            }
        }

    }
}
