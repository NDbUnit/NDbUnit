using System;
using NUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [Category(TestCategories.SqlServerTests)]
    [TestFixture]
    public class SchemaPrefixTest
    {
        [Test]
        public void Can_Perform_CleanInsertUpdate_Operation_Without_Exception_When_Schema_Has_Prefix()
        {
            try
            {
                var db = new NDbUnit.Core.SqlClient.SqlDbUnitTest(DbConnection.SqlConnectionString);
                db.ReadXmlSchema(XmlTestFiles.SqlServer.XmlSchemaFileForSchemaPrefixTests);
                db.ReadXml(XmlTestFiles.SqlServer.XmlFileForSchemaPrefixTests);

                db.PerformDbOperation(NDbUnit.Core.DbOperationFlag.CleanInsertIdentity);
            }
            catch (Exception)
            {
                Assert.Fail("Operation not successful when using tables with Schema Prefixes.");
            }

        }
    }
}