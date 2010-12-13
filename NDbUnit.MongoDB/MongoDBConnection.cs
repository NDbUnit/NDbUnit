using System;
using System.Data;
using MongoDB.Driver;
using System.Collections.Generic;

namespace NDbUnit.Core.MongoDB
{
    public class MongoDBConnection : IDbConnection
    {
        private MongoServer _mongoServer;
        private string _database;
        private string _connectionString;
        
        private readonly Dictionary<MongoServerState, ConnectionState> _mongoServerToConnectionXRef;



        public MongoDBConnection()
        {
            _mongoServerToConnectionXRef = new Dictionary<MongoServerState, ConnectionState>()
                                               {
                                                   {MongoServerState.Connected, ConnectionState.Open},
                                                   {MongoServerState.Connecting, ConnectionState.Connecting},
                                                   {MongoServerState.Disconnected, ConnectionState.Closed},
                                                   {MongoServerState.None, ConnectionState.Broken}
                                               };
            ConnectionTimeout = 30;

        }

        public MongoDBConnection(string connectionString) : this()
        {
            _connectionString = connectionString;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            _mongoServer.Disconnect();
        }

        public void ChangeDatabase(string databaseName)
        {
            _database = databaseName;
            _mongoServer.GetDatabase(databaseName);
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            _mongoServer = MongoServer.Create(ConnectionString);
            _mongoServer.Connect(System.TimeSpan.FromSeconds(ConnectionTimeout));
        }

        public string ConnectionString
        {
            get { return _connectionString; } 
            set
            {
                if (value == _connectionString)
                    return;
                
                if (this.State != ConnectionState.Closed) 
                    this.Close();

                _connectionString = value;
            }  
        }

        public int ConnectionTimeout { get; set; }

        public string Database
        {
            get { return _database; }
        }

        public ConnectionState State
        {
            get
            {
                if (_mongoServer == null)
                    return ConnectionState.Closed;

                return _mongoServerToConnectionXRef[_mongoServer.State];
            }
        }
    }
}