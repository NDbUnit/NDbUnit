using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using NDbUnit.Core;

namespace NDbUnit.Test.MongoDB
{
    [TestFixture]
    public class MongoDBIntegrationTest : NDbUnit.Test.Common.DbOperationTestBase
    {
        protected override IDbCommandBuilder GetCommandBuilder()
        {
            throw new NotImplementedException();
        }

        protected override IDbOperation GetDbOperation()
        {
            throw new NotImplementedException();
        }

        protected override IDbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
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
