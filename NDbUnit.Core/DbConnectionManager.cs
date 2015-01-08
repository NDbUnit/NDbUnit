using System;
using System.Data;
using System.Data.Odbc;

namespace NDbUnit.Core
{
    public class DbConnectionManager<TDbConnection> where TDbConnection : IDbConnection, new()
    {
        private IDbConnection _connection;
        private readonly string _connectionString;
        private readonly bool _externallyManagedConnection;

        public DbConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnectionManager(IDbConnection connection)
        {
            _externallyManagedConnection = true;
            _connectionString = connection.ConnectionString;
            _connection = connection;
        }

        public IDbConnection GetConnection(bool forceNewConnection = false)
        {
            if (forceNewConnection)
            {
                if (_externallyManagedConnection)
                {
                    throw new InvalidOperationException("Cannot force new connection when DbConnectionManager has been initialized with an external connection.");
                }

                ReleaseConnection();
            }

            if (null == _connection && !_externallyManagedConnection)
            {
                _connection = CreateConnection(_connectionString);
            }
            
            return _connection;
        }

        public void ReleaseConnection()
        {
            if (_connection != null && !_externallyManagedConnection)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public IDbConnection CreateConnection(string connectionString)
        {
            return new TDbConnection { ConnectionString = _connectionString };
        }
    }
}