using System;
using System.Data;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBCommandBuilder : DbCommandBuilder
    {

        public MongoDBCommandBuilder(IDbConnection connection)
            : base(connection)
        {
        }

        public MongoDBCommandBuilder(string connectionString)
            : base(connectionString)
        {
        }

        protected override IDbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        protected override IDbConnection GetConnection(string connectionString)
        {
            return new MongoDBConnection(connectionString);
        }
    }
}
