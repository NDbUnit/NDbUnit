using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.Data;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class PerformDBOperationOnNorthwindFailsBugReportTest
    {

        NDbUnit.Core.INDbUnitTest _sql = new NDbUnit.Core.SqlClient.SqlDbUnitTest(@"Data Source=localhost\sqlserver2005;Initial Catalog=Northwind;User ID=sa;Password=aHJaeNN4");


        [FixtureSetUp]
        public void TestFixtureSetup()
        {
            _sql.ReadXmlSchema(@"..\..\xml\northwind.xsd");
            _sql.ReadXml(@"..\..\xml\northwind_backup.xml");
            _sql.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);
        }

        [Test]
        public void CanGetDataSetFromDB()
        {
            DataSet ds = _sql.GetDataSetFromDb();

            ds.WriteXml(@"..\..\xml\northwind.xml");

            _sql.ReadXml(@"..\..\xml\northwind.xml");            
        }


        [Test]
        public void CanDeleteAll()
        {
            DataSet ds = _sql.GetDataSetFromDb();

            ds.WriteXml(@"..\..\xml\northwind.xml");

            _sql.ReadXml(@"..\..\xml\northwind.xml");

            _sql.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);
        }


        [Test]
        public void Test()
        {
            Assert.IsTrue(true);
        }

    }


}
