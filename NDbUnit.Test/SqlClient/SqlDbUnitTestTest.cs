using System;
using MbUnit.Framework;
using NDbUnit.Core;
using System.Data;
using System.IO;
using NDbUnit.Core.SqlClient;
using System.Collections.Generic;

namespace NDbUnit.Test.SqlServerCe
{
    [TestFixture]
    public class SqlDbUnitTestTest : NDbUnit.Test.Common.DbUnitTestTestBase
    {
        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServer.XmlSchemaFile;
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServer.XmlFile;
        }

        protected override IUnitTestStub GetUnitTestStub()
        {
            return new SqlUnitTestStub(DbConnection.SqlConnectionString);
        }

        protected class SqlUnitTestStub : SqlDbUnitTest, IUnitTestStub
        {
            public SqlUnitTestStub(string connectionString)
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

        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "dbo.User", "UserRole" 
                };
            }
        }
    }

}


