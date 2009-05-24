using System;
using MbUnit.Framework;
using NDbUnit.Core.SqlServerCe;
using System.Data;
using System.Data.SqlServerCe;

namespace NDbUnit.Test.SqlServerCe
{
    [TestFixture]
    class SqlCeDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlCeDbCommandBuilder(DbConnection.SqlCeConnectionString);
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlCeDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "ALTER TABLE [" + table.TableName + "] ALTER COLUMN [" + column.ColumnName +
                                         "] IDENTITY (1,1)";
            return new SqlCeCommand(sql, (SqlCeConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServerCe.XmlSchemaFile;
        }

    }
}
