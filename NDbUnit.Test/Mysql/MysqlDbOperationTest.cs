using System;
using MbUnit.Framework;
using NDbUnit.Core.MySqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace NDbUnit.Test.Mysql
{
    [TestFixture]
    class MySqlDbOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new MySqlDbCommandBuilder(DbConnection.MySqlConnectionString);
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new MySqlDbOperation();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            String sql = "ALTER TABLE " + table.TableName + " AUTO_INCREMENT=1;";
            return new MySqlCommand(sql, (MySqlConnection)_commandBuilder.Connection);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.MySql.XmlFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.MySql.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.MySql.XmlSchemaFile;
        }

    }

}