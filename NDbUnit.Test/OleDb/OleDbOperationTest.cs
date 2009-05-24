using System;
using MbUnit.Framework;
using System.Data;
using NDbUnit.Core.OleDb;
using System.Data.OleDb;

namespace NDbUnit.Test.OleDb
{
    [TestFixture]
    public class OleDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new NDbUnit.Core.OleDb.OleDbCommandBuilder(DbConnection.OleDbConnectionString);
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new OleDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "dbcc checkident([" + table.TableName + "], RESEED, 0)";
            return new OleDbCommand(sql, (OleDbConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OleDb.XmlFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.OleDb.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
