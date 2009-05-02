/*
 *
 * NDbUnit
 * Copyright (C)2005
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
using MbUnit.Framework;
using System.Data;
using System.IO;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class PerformDBOperationOnNorthwindFailsBugReportTest
    {
        private const string NORTHWIND_NEW_XML_FILE = @"..\..\xml\northwind.xml";
        private const string NORTHWIND_XSD_FILE = @"..\..\xml\northwind.xsd";
        private const string NORTHWIND_BACKUP_XML_FILE = @"..\..\xml\northwind_backup.xml";

        NDbUnit.Core.INDbUnitTest _sql = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlNorthwindConnectionString);


        [SetUp]
        public void TestSetup()
        {
            _sql.ReadXmlSchema(NORTHWIND_XSD_FILE);
            _sql.ReadXml(NORTHWIND_BACKUP_XML_FILE);
            _sql.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);

        }

        [Test]
        public void CanGetDataSetFromDB()
        {
            DataSet ds = _sql.GetDataSetFromDb();

            if (File.Exists(NORTHWIND_NEW_XML_FILE))
                File.Delete(NORTHWIND_NEW_XML_FILE);

            ds.WriteXml(NORTHWIND_NEW_XML_FILE);

            Assert.IsTrue(File.Exists(NORTHWIND_NEW_XML_FILE));

            //10 bytes means SOMETHING of import was written to disk...
            Assert.GreaterThan<double>(new FileInfo(NORTHWIND_NEW_XML_FILE).Length, 1000);
        }


        [Test]
        public void CanDeleteAll()
        {
            _sql.PerformDbOperation(NDbUnit.Core.DbOperationFlag.DeleteAll);

            DataSet ds = _sql.GetDataSetFromDb();

            ds.WriteXml(NORTHWIND_NEW_XML_FILE);

            Assert.LessThan<double>(new FileInfo(NORTHWIND_NEW_XML_FILE).Length, 1000);


        }

          

    }


}
