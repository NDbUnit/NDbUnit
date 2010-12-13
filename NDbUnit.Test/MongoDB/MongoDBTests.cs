using System.Data;
using MbUnit.Framework;
using NDbUnit.Core.MongoDB;

namespace NDbUnit.Test
{
    [TestFixture]
    public class MongoDBTests
    {
     
        [Test]
        public void CanCreateMongoDBCommandBuilder()
        {
            var mongoDbCommandBuilder = new MongoDBCommandBuilder(DbConnection.MongoDBConnectionString);

            Assert.IsNotNull(mongoDbCommandBuilder);

            mongoDbCommandBuilder.Connection.Open();

            Assert.AreEqual(ConnectionState.Open,mongoDbCommandBuilder.Connection.State);

            mongoDbCommandBuilder.Connection.Close();

            Assert.AreEqual(ConnectionState.Closed, mongoDbCommandBuilder.Connection.State);

        }

        [Test]
        public void CanOpenCloseMongoDBConnection()
        {
            var connection = new MongoDBCommandBuilder(DbConnection.MongoDBConnectionString).Connection;

            connection.Open();

            Assert.AreEqual(ConnectionState.Open, connection.State);

            connection.Close();

            Assert.AreEqual(ConnectionState.Closed, connection.State);
        }
    }
}
