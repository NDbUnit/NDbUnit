using System;
using System.Data;
using System.IO;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBCommandBuilder : IDbCommandBuilder
    {
        public int CommandTimeOutSeconds
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string QuotePrefix
        {
            get { throw new NotImplementedException(); }
        }

        public string QuoteSuffix
        {
            get { throw new NotImplementedException(); }
        }

        public IDbConnection Connection
        {
            get { throw new NotImplementedException(); }
        }

        public DataSet GetSchema()
        {
            throw new NotImplementedException();
        }

        public void BuildCommands(string xmlSchemaFile)
        {
            throw new NotImplementedException();
        }

        public void BuildCommands(Stream xmlSchema)
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetSelectCommand(string tableName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetInsertCommand(string tableName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetInsertIdentityCommand(string tableName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetDeleteCommand(string tableName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetDeleteAllCommand(string tableName)
        {
            throw new NotImplementedException();
        }

        public IDbCommand GetUpdateCommand(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}
