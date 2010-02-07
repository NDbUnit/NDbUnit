using System;
using System.Collections.Generic;
using System.Text;
using NDbUnit.Core.SqlClient;
using NDbUnit.Core.OleDb;

namespace NDbUnit.Test.SqlClient
{
    public class OleDbUnitIntegrationTest : IntegationTestBase
    {
        protected override NDbUnit.Core.INDbUnitTest GetNDbUnitTest()
        {
            return new OleDbUnitTest(DbConnection.OleDbConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OleDb.XmlFile;
        }

        protected override string GetXmlModFilename()
        {
            return XmlTestFiles.OleDb.XmlModFile;
        }

        protected override string GetXmlRefreshFilename()
        {
            return XmlTestFiles.OleDb.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OleDb.XmlSchemaFile;
        }

    }
}
