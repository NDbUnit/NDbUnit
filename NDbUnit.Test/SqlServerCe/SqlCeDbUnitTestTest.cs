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
using System.Data;
using System.Data.Common;
using System.IO;
using NDbUnit.Core;
using NDbUnit.Core.SqlServerCe;
using MbUnit.Framework;
using Rhino.Mocks;
using Is=Rhino.Mocks.Constraints.Is;

namespace NDbUnit.Test.SqlServerCe
{
    [TestFixture]
    public class SqlCeDbUnitTestTestCase
    {
        private TestableSqlCeUnitTest sqlCeTest;
        private MockRepository mocker;
        private static IDbCommandBuilder mockDbCommandBuilder;
        private static IDbOperation mockDbOperation;
        private static FileStream mockSchemaFileStream;
        private static FileStream mockDataFileStream;
        private IDbConnection mockConnection;
        private bool preOperationCalled;
        private bool postOperationCalled;
        private IDbTransaction mockTransaction;

        [SetUp]
        public void SetUp()
        {
            mockSchemaFileStream = new FileStream(XmlTestFiles.SqlServerCe.XmlSchemaFile, FileMode.Open,
                                                  FileAccess.Read, FileShare.Read);

            mockDataFileStream = new FileStream(XmlTestFiles.SqlServerCe.XmlFile, FileMode.Open);

            mocker = new MockRepository();
            mockDbCommandBuilder = mocker.CreateMock<IDbCommandBuilder>();
            mockDbOperation = mocker.CreateMock<IDbOperation>();
            sqlCeTest = new TestableSqlCeUnitTest(DbConnection.SqlCeConnectionString);
            mockConnection = mocker.CreateMock<IDbConnection>();
            mockTransaction = mocker.CreateMock<IDbTransaction>();
        }

        [TearDown]
        public void TearDown()
        {
            mocker.ReplayAll();
            mocker.VerifyAll();

            mockSchemaFileStream.Close();
            mockDataFileStream.Close();
        }

        [Test]
        [ExpectedException(typeof (NDbUnitException))]
        public void TestCopyDataSetWhenIsNotInitializedThrowsException()
        {
            sqlCeTest.CopyDataSet();
        }

        [Test]
        [ExpectedException(typeof (NDbUnitException))]
        public void TestCopySchemaWhenIsNotInitializedThrowsException()
        {
            sqlCeTest.CopySchema();
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestReadXmlSchemaWithEmptyStringThrowsException()
        {
            sqlCeTest.ReadXmlSchema("");
        }

        [Test]
        public void TestReadXmlSchemaInitializesDataSet()
        {
            //expectations
            mockDbCommandBuilder.BuildCommands(mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.Tables.Add("dummyTable");
            SetupResult.For(mockDbCommandBuilder.GetSchema()).Return(dummyDS);

            mocker.ReplayAll();
            sqlCeTest.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            DataSet copyOfDataSet = sqlCeTest.TestDataSet;
            Assert.IsNotNull(copyOfDataSet.Tables["dummyTable"], "Expected to see dummy table");
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void TestReadXmlDataFileWithoutFirstCallingReadXmlSchemaThrowsException()
        {
            mocker.ReplayAll();
            sqlCeTest.ReadXml(XmlTestFiles.SqlServerCe.XmlFile);
        }

        [Test]
        public void TestReadXmlDataFile()
        {
            //expectations
            mockDbCommandBuilder.BuildCommands(mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            SetupResult.For(mockDbCommandBuilder.GetSchema()).Return(dummyDS);

            mocker.ReplayAll();
            sqlCeTest.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            sqlCeTest.ReadXml(XmlTestFiles.SqlServerCe.XmlFile);
            DataSet copyOfDataSet = sqlCeTest.TestDataSet;
            Assert.AreEqual(3, copyOfDataSet.Tables.Count, "Expected 3 tables");
            Assert.AreEqual("Role", copyOfDataSet.Tables[0].TableName, "Wrong table");
            Assert.AreEqual("User", copyOfDataSet.Tables[1].TableName, "Wrong table");
            Assert.AreEqual("UserRole", copyOfDataSet.Tables[2].TableName, "Wrong table");
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void TestReadXmlDataFileWithEmptyStringThrowsException()
        {
            sqlCeTest.ReadXml("");
        }

        [Test]
        [ExpectedException(typeof (NDbUnitException))]
        public void TestPerformDbOperationWhenNotInitializedThrowsException()
        {
            sqlCeTest.PerformDbOperation(DbOperationFlag.Update);
        }

        [Test]
        public void TestPerformDbOperation()
        {
            sqlCeTest.PreOperation += new PreOperationEvent(sqlCeTest_PreOperation);
            sqlCeTest.PostOperation += new PostOperationEvent(sqlCeTest_PostOperation);

            //expectations
            mockDbCommandBuilder.BuildCommands(mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            SetupResult.For(mockDbCommandBuilder.GetSchema()).Return(dummyDS);
            SetupResult.For(mockDbCommandBuilder.Connection).Return(mockConnection);
            mockConnection.Open();
            SetupResult.For(mockConnection.BeginTransaction()).Return(mockTransaction);
            mockDbOperation.Update(dummyDS, mockDbCommandBuilder, mockTransaction);
            LastCall.IgnoreArguments().Constraints(Is.TypeOf<DataSet>(), Is.Equal(mockDbCommandBuilder),
                                                   Is.Equal(mockTransaction));
            mockTransaction.Commit();
            SetupResult.For(mockConnection.State).Return(ConnectionState.Open);
            mockConnection.Close();

            //end expectations

            mocker.ReplayAll();
            sqlCeTest.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            sqlCeTest.ReadXml(XmlTestFiles.SqlServerCe.XmlFile);
            sqlCeTest.PerformDbOperation(DbOperationFlag.Update);

            DataSet copyOfDataSet = sqlCeTest.TestDataSet;

            Assert.AreEqual(3, copyOfDataSet.Tables.Count, "Expected 3 tables");
            Assert.AreEqual("Role", copyOfDataSet.Tables[0].TableName, "Wrong table");
            Assert.AreEqual("User", copyOfDataSet.Tables[1].TableName, "Wrong table");
            Assert.AreEqual("UserRole", copyOfDataSet.Tables[2].TableName, "Wrong table");

            Assert.IsTrue(preOperationCalled, "PreOperation() callback was not fired.");
            Assert.IsTrue(postOperationCalled, "PostOperation() callback was not fired.");
        }

        private void sqlCeTest_PostOperation(object sender, OperationEventArgs args)
        {
            postOperationCalled = true;
        }

        private void sqlCeTest_PreOperation(object sender, OperationEventArgs args)
        {
            preOperationCalled = true;
        }


        private class TestableSqlCeUnitTest : SqlCeUnitTest
        {
            public TestableSqlCeUnitTest(string connectionString) : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
            {
                return mockDbCommandBuilder;
            }

            protected override IDbOperation CreateDbOperation()
            {
                return mockDbOperation;
            }

            protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
            {
                return base.CreateDataAdapter(command);
            }

            protected override FileStream GetXmlSchemaFileStream(string xmlSchemaFile)
            {
                return mockSchemaFileStream;
            }

            protected override FileStream GetXmlDataFileStream(string xmlFile)
            {
                return mockDataFileStream;
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
