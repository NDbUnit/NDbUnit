/*
 *
 * NDbUnit
 * Copyright (C)2005 - 2010
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
