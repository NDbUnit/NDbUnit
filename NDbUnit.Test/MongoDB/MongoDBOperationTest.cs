using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NDbUnit.Core.MongoDB;

namespace NDbUnit.Test.MongoDB
{
    [TestFixture]
    public class MongoDBOperationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new MongoDBCommandBuilder(DbConnection.MongoDBConnectionString);
        }

        protected override Core.IDbOperation GetDbOperation()
        {
            return new MongoDBOperation();
        }

        protected override System.Data.IDbCommand GetResetIdentityColumnsDbCommand(System.Data.DataTable table, System.Data.DataColumn column)
        {
            throw new NotImplementedException();
        }

        protected override string GetXmlFilename()
        {
            throw new NotImplementedException();
        }

        protected override string GetXmlModifyFilename()
        {
            throw new NotImplementedException();
        }

        protected override string GetXmlRefeshFilename()
        {
            throw new NotImplementedException();
        }

        protected override string GetXmlSchemaFilename()
        {
            throw new NotImplementedException();
        }
    }
}
