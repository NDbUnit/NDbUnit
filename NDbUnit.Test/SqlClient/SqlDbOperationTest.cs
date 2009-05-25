using System;
using MbUnit.Framework;
using NDbUnit.Core.SqlClient;
using System.Data;
using System.Data.SqlClient;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class SqlDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlDbCommandBuilder(DbConnection.SqlConnectionString);
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "dbcc checkident([" + table.TableName + "], RESEED, 0)";
            return new SqlCommand(sql, (SqlConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServer.XmlFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.SqlServer.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServer.XmlSchemaFile;
        }

    }
}
