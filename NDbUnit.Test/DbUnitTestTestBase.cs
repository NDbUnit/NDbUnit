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
using System.Data;
using System.Data.Common;
using System.IO;
using NDbUnit.Core;
using NDbUnit.Core.SqlServerCe;
using MbUnit.Framework;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;
using System.Collections.Generic;

namespace NDbUnit.Test.Common
{

    public abstract class DbUnitTestTestBase
    {
        private const int EXPECTED_COUNT_OF_COMMANDS = 3;

        protected IDbConnection _mockConnection;

        protected static FileStream _mockDataFileStream;

        protected static IDbCommandBuilder _mockDbCommandBuilder;

        protected static IDbOperation _mockDbOperation;

        protected MockRepository _mocker;

        protected static FileStream _mockSchemaFileStream;

        protected IDbTransaction _mockTransaction;

        protected IUnitTestStub _nDbUnitTestStub;

        protected bool _postOperationCalled;

        protected bool _preOperationCalled;

        public abstract IList<string> ExpectedDataSetTableNames { get; }

        [SetUp]
        public void _SetUp()
        {
            _mockSchemaFileStream = new FileStream(GetXmlSchemaFilename(), FileMode.Open,
                                                  FileAccess.Read, FileShare.Read);

            _mockDataFileStream = new FileStream(GetXmlFilename(), FileMode.Open);

            _mocker = new MockRepository();
            _mockDbCommandBuilder = _mocker.CreateMock<IDbCommandBuilder>();
            _mockDbOperation = _mocker.CreateMock<IDbOperation>();
            _nDbUnitTestStub = GetUnitTestStub();
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
            Assert.Throws<NDbUnitException>(() => _nDbUnitTestStub.CopyDataSet());
        }

        [Test]

        public void CopySchema_When_DataSet_Not_Initialized_Throws_Exception()
        {
            Assert.Throws<NDbUnitException>(() => _nDbUnitTestStub.CopySchema());
        }

        [Test]
        public void PerformDbOperation_Raises_PreOperation_and_PostOperation_Events()
        {
            _nDbUnitTestStub.PreOperation += new PreOperationEvent(sqlCeTest_PreOperation);
            _nDbUnitTestStub.PostOperation += new PostOperationEvent(sqlCeTest_PostOperation);

            //expectations
            _mockDbCommandBuilder.BuildCommands(_mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.ReadXmlSchema(GetXmlSchemaFilename());
            SetupResult.For(_mockDbCommandBuilder.GetSchema()).Return(dummyDS);
            SetupResult.For(_mockDbCommandBuilder.Connection).Return(_mockConnection);
            //_mockConnection.Open();
            SetupResult.For(_mockConnection.BeginTransaction()).Return(_mockTransaction);
            _mockDbOperation.Update(dummyDS, _mockDbCommandBuilder, _mockTransaction);
            LastCall.IgnoreArguments().Constraints(Is.TypeOf<DataSet>(), Is.Equal(_mockDbCommandBuilder),
                                                   Is.Equal(_mockTransaction));
            _mockTransaction.Commit();
            SetupResult.For(_mockConnection.State).Return(ConnectionState.Open);
            _mockConnection.Close();

            //end expectations

            _mocker.ReplayAll();
            _nDbUnitTestStub.ReadXmlSchema(GetXmlSchemaFilename());
            _nDbUnitTestStub.ReadXml(GetXmlFilename());
            _nDbUnitTestStub.PerformDbOperation(DbOperationFlag.Update);

            Assert.IsTrue(_preOperationCalled, "PreOperation() callback was not fired.");
            Assert.IsTrue(_postOperationCalled, "PostOperation() callback was not fired.");
        }

        [Test]
        public void PerformDbOperation_When_Not_Initialized_Throws_Exception()
        {
            Assert.Throws<NDbUnitException>(() => _nDbUnitTestStub.PerformDbOperation(DbOperationFlag.Update));
        }

        [Test]
        public void ReadXml_From_DataSet_File_Loads_Tables()
        {
            //expectations
            _mockDbCommandBuilder.BuildCommands(_mockSchemaFileStream);
            DataSet dummyDS = new DataSet();
            dummyDS.ReadXmlSchema(GetXmlSchemaFilename());
            SetupResult.For(_mockDbCommandBuilder.GetSchema()).Return(dummyDS);
            _mocker.ReplayAll();

            _nDbUnitTestStub.ReadXmlSchema(GetXmlSchemaFilename());
            _nDbUnitTestStub.ReadXml(GetXmlFilename());
            DataSet copyOfDataSet = _nDbUnitTestStub.TestDataSet;

            IList<string> schemaTables = new List<string>();

            foreach (DataTable dataTable in copyOfDataSet.Tables)
            {
                schemaTables.Add(dataTable.TableName);

                Console.WriteLine(String.Format("Table '{0}' found in dataset", dataTable.TableName));
            }

            Assert.AreEqual(EXPECTED_COUNT_OF_COMMANDS, copyOfDataSet.Tables.Count, string.Format("Should be {0} Tables in dataset", EXPECTED_COUNT_OF_COMMANDS));
            Assert.AreElementsEqual<string>(ExpectedDataSetTableNames, schemaTables);
        }

        [Test]
        public void ReadXml_From_DataSet_File_With_Empty_String_Throws_Exception()
        {
            Assert.Throws<ArgumentException>(() => _nDbUnitTestStub.ReadXml(""));
        }

        [Test]
        public void ReadXml_From_DataSet_File_Without_First_Calling_ReadXmlSchema_Throws_Exception()
        {
            _mocker.ReplayAll();
            Assert.Throws<InvalidOperationException>(() => _nDbUnitTestStub.ReadXml(GetXmlFilename()));
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
            _nDbUnitTestStub.ReadXmlSchema(GetXmlSchemaFilename());
            DataSet copyOfDataSet = _nDbUnitTestStub.TestDataSet;
            Assert.IsNotNull(copyOfDataSet.Tables["dummyTable"], "Expected to see dummy table");
        }

        [Test]
        public void ReadXmlSchema_With_Empty_String_Throws_Exception()
        {
            Assert.Throws<ArgumentException>(() => _nDbUnitTestStub.ReadXmlSchema(""));
        }

        protected abstract IUnitTestStub GetUnitTestStub();

        protected abstract string GetXmlFilename();

        protected abstract string GetXmlSchemaFilename();

        private void sqlCeTest_PostOperation(object sender, OperationEventArgs args)
        {
            _postOperationCalled = true;
        }

        private void sqlCeTest_PreOperation(object sender, OperationEventArgs args)
        {
            _preOperationCalled = true;
        }

        protected interface IUnitTestStub : INDbUnitTest
        {
            DataSet TestDataSet { get; }

        }

    }
}
