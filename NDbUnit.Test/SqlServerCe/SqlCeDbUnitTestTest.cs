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
using Is = Rhino.Mocks.Constraints.Is;

namespace NDbUnit.Test.SqlServerCe
{
    [TestFixture]
    public class SqlCeDbUnitTestTestCase
    {
        private IDbConnection _mockConnection;

        private static FileStream _mockDataFileStream;

        private static IDbCommandBuilder _mockDbCommandBuilder;

        private static IDbOperation _mockDbOperation;

        private MockRepository _mocker;

        private static FileStream _mockSchemaFileStream;

        private IDbTransaction _mockTransaction;

        private bool _postOperationCalled;

        private bool _preOperationCalled;

        private SqlCeUnitTestStub _sqlCeTest;

        [SetUp]
        public void _SetUp()
        {
            _mockSchemaFileStream = new FileStream(XmlTestFiles.SqlServerCe.XmlSchemaFile, FileMode.Open,
                                                  FileAccess.Read, FileShare.Read);

            _mockDataFileStream = new FileStream(XmlTestFiles.SqlServerCe.XmlFile, FileMode.Open);

            _mocker = new MockRepository();
            _mockDbCommandBuilder = _mocker.CreateMock<IDbCommandBuilder>();
            _mockDbOperation = _mocker.CreateMock<IDbOperation>();
            _sqlCeTest = new SqlCeUnitTestStub(DbConnection.SqlCeConnectionString);
            _mockConnection = _mocker.CreateMock<IDbConnection>();
            _mockTransaction = _mocker.CreateMock<IDbTransaction>();
        }

        [TearDown]
        public void _TearDown()
        {
            _mocker.ReplayAll();
            _mocker.VerifyAll();

            _mockSchemaFileStream.Close();
            _mockDataFileStream.Close();
        }

        [Test]
        public void CopyDataSet_When_DataSet_Not_Initialized_Throws_Exception()
        {
            try
            {
                _sqlCeTest.CopyDataSet();
                Assert.Fail("Expected Exception of Type NDbUnitException not Thrown!");
            }
            catch (NDbUnitException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]

        public void CopySchema_When_DataSet_Not_Initialized_Throws_Exception()
        {
            try
            {
                _sqlCeTest.CopySchema();
                Assert.Fail("Expected Exception of Type NDbUnitException not Thrown!");
            }
            catch (NDbUnitException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void PerformDbOperation_Raises_PreOperation_and_PostOperation_Events()
        {
            _sqlCeTest.PreOperation += new PreOperationEvent(sqlCeTest_PreOperation);
            _sqlCeTest.PostOperation += new PostOperationEvent(sqlCeTest_PostOperation);

            //expectations
            _mockDbCommandBuilder.BuildCommands(_mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            SetupResult.For(_mockDbCommandBuilder.GetSchema()).Return(dummyDS);
            SetupResult.For(_mockDbCommandBuilder.Connection).Return(_mockConnection);
            _mockConnection.Open();
            SetupResult.For(_mockConnection.BeginTransaction()).Return(_mockTransaction);
            _mockDbOperation.Update(dummyDS, _mockDbCommandBuilder, _mockTransaction);
            LastCall.IgnoreArguments().Constraints(Is.TypeOf<DataSet>(), Is.Equal(_mockDbCommandBuilder),
                                                   Is.Equal(_mockTransaction));
            _mockTransaction.Commit();
            SetupResult.For(_mockConnection.State).Return(ConnectionState.Open);
            _mockConnection.Close();

            //end expectations

            _mocker.ReplayAll();
            _sqlCeTest.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            _sqlCeTest.ReadXml(XmlTestFiles.SqlServerCe.XmlFile);
            _sqlCeTest.PerformDbOperation(DbOperationFlag.Update);

            Assert.IsTrue(_preOperationCalled, "PreOperation() callback was not fired.");
            Assert.IsTrue(_postOperationCalled, "PostOperation() callback was not fired.");
        }

        [Test]
        public void PerformDbOperation_When_Not_Initialized_Throws_Exception()
        {
            try
            {
                _sqlCeTest.PerformDbOperation(DbOperationFlag.Update);
                Assert.Fail("Expected Exception of type NDbUnitException was not thrown!");
            }
            catch (NDbUnitException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ReadXml_From_DataSet_File_Loads_Tables()
        {
            //expectations
            _mockDbCommandBuilder.BuildCommands(_mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            SetupResult.For(_mockDbCommandBuilder.GetSchema()).Return(dummyDS);
            _mocker.ReplayAll();

            _sqlCeTest.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            _sqlCeTest.ReadXml(XmlTestFiles.SqlServerCe.XmlFile);
            DataSet copyOfDataSet = _sqlCeTest.TestDataSet;
            Assert.AreEqual(3, copyOfDataSet.Tables.Count, "Expected 3 tables");
            Assert.AreEqual("Role", copyOfDataSet.Tables[0].TableName, "Wrong table");
            Assert.AreEqual("User", copyOfDataSet.Tables[1].TableName, "Wrong table");
            Assert.AreEqual("UserRole", copyOfDataSet.Tables[2].TableName, "Wrong table");
        }

        [Test]
        public void ReadXml_From_DataSet_File_With_Empty_String_Throws_Exception()
        {
            try
            {
                _sqlCeTest.ReadXml("");
                Assert.Fail("Expected Exception of Type ArgumentException was not Thrown!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ReadXml_From_DataSet_File_Without_First_Calling_ReadXmlSchema_Throws_Exception()
        {
            _mocker.ReplayAll();
            try
            {
                _sqlCeTest.ReadXml(XmlTestFiles.SqlServerCe.XmlFile);
                Assert.Fail("Expected Exception of Type InvalidOperationException was not Thrown!");
            }
            catch (InvalidOperationException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [Test]
        public void ReadXmlSchema_Initializes_DataSet()
        {
            //expectations
            _mockDbCommandBuilder.BuildCommands(_mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.Tables.Add("dummyTable");
            SetupResult.For(_mockDbCommandBuilder.GetSchema()).Return(dummyDS);

            _mocker.ReplayAll();
            _sqlCeTest.ReadXmlSchema(XmlTestFiles.SqlServerCe.XmlSchemaFile);
            DataSet copyOfDataSet = _sqlCeTest.TestDataSet;
            Assert.IsNotNull(copyOfDataSet.Tables["dummyTable"], "Expected to see dummy table");
        }

        [Test]
        public void ReadXmlSchema_With_Empty_String_Throws_Exception()
        {
            try
            {
                _sqlCeTest.ReadXmlSchema("");
                Assert.Fail("Expected Exception of Type ArgumentException was not Thrown!");
            }
            catch (ArgumentException ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        private void sqlCeTest_PostOperation(object sender, OperationEventArgs args)
        {
            _postOperationCalled = true;
        }

        private void sqlCeTest_PreOperation(object sender, OperationEventArgs args)
        {
            _preOperationCalled = true;
        }

        private class SqlCeUnitTestStub : SqlCeUnitTest
        {
            public SqlCeUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
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
