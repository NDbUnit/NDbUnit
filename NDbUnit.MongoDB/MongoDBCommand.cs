using System;
using System.Data;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBCommand : IDbCommand
    {
    
        
        public MongoDBCommand(string cmdText)
        {
            CommandText = cmdText;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public IDbConnection Connection { get; set; }

        public IDbTransaction Transaction { get; set; }

        public string CommandText { get; set; }

        public int CommandTimeout { get; set; }

        public CommandType CommandType { get; set; }

        public IDataParameterCollection Parameters { get { return null; } }

        public UpdateRowSource UpdatedRowSource { get; set; }
    }
}
