using System;
using NUnit.Framework;

namespace NDbUnit.Test.SqlClient
{
    [TestFixture]
    public class SchemaPrefixTest
    {

        [TestFixture]
        public class When_Schema_XSD_File_Contains_DBSchema_Prefixes
        {
            [Test]
            public void Can_Perform_CleanInsertUpdate_Operation_Without_Exception()
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
}
