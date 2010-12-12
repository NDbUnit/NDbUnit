using MbUnit.Framework;
using NDbUnit.Core.MongoDB;

namespace NDbUnit.MongoDB
{
    [TestFixture]
    public class tests
    {
        [Test]
        public void CanCreateMongoDBCommandBuilder()
        {
            var mongoDbCommandBuilder = new MongoDBCommandBuilder("mongodb://localhost");
        }
    }
}
