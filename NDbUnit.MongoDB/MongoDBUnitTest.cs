using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBUnitTest : NDbUnitTest
    {
        public MongoDBUnitTest(string connectionString) : base(connectionString)
        {
        }

        public MongoDBUnitTest(IDbConnection connection) : base(connection)
        {
        }

        protected override IDbDataAdapter CreateDataAdapter(IDbCommand command)
        {
            throw new NotImplementedException();
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(string connectionString)
        {
            throw new NotImplementedException();
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        protected override IDbOperation CreateDbOperation()
        {
            throw new NotImplementedException();
        }
    }
}
