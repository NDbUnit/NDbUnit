using System;
using System.Collections.Generic;
using MbUnit.Framework;
using NDbUnit.Core;
using System.Data;
using System.IO;
using NDbUnit.Core.SqlLite;

namespace NDbUnit.Test.SqlLite
{
    [TestFixture]
    public class SqlLiteUnitTestTest : NDbUnit.Test.Common.DbUnitTestTestBase
    {
        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Sqlite.XmlSchemaFile;
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Sqlite.XmlFile;
        }

        protected override IUnitTestStub GetUnitTestStub()
        {
            return new SqliteUnitTestStub(DbConnection.SqlLiteConnectionString);
        }

        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "User", "UserRole" 
                };
            }
        }

        protected class SqliteUnitTestStub : SqlLiteUnitTest, IUnitTestStub
        {
            public SqliteUnitTestStub(string connectionString)
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


