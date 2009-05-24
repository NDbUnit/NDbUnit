using System;
using MbUnit.Framework;
using NDbUnit.Core.SqlLite;
using System.Data;
using System.Data.SQLite;

namespace NDbUnit.Test.SqlLite
{
    [TestFixture]
    public class SqlLiteDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new SqlLiteDbCommandBuilder(DbConnection.SqlLiteConnectionString);
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new SqlLiteDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "delete from sqlite_sequence where name = '" + table.TableName + "'";
            return new SQLiteCommand(sql, (SQLiteConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.Sqlite.XmlFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.Sqlite.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.Sqlite.XmlSchemaFile;
        }

    }
}
