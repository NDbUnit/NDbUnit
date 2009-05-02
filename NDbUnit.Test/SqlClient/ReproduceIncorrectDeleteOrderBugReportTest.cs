using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class ReproduceIncorrectDeleteOrderBugReportTest
    {

        NDbUnit.Core.INDbUnitTest _sql = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);

        [FixtureSetUp]
        public void TestFixtureSetup()
        {
            _sql.ReadXmlSchema(@"..\..\xml\userds.xsd");
            _sql.ReadXml(@"..\..\xml\user.xml");
        }


        [TearDown]
        public void TearDown()
        {
            _sql.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);
        }

        [SetUp]
        public void Setup()
        {
            _sql.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);

        }

        [Test]
        public void Test()
        {
            
        }
    }
}
