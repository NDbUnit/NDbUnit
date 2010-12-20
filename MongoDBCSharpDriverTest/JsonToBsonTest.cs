
using System;
using System.IO;
using System.Reflection;
using MongoDB.Bson;
using NUnit.Framework;
using MongoDB.Driver;

namespace MongoDBCSharpDriverTest
{
    [TestFixture]
    public class JsonToBsonTest
    {
        private string jsonText;
        private string jsonText2;
        private string jsonText3;

        [TestFixtureSetUp]
        void Setup()
        {
            jsonText = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json5.txt")).ReadToEnd();
        
        }

        [Test]
        public void CanConvertJsonToBson()
        {
            var bsonDoc = JsonToBson.ToBson(jsonText);

            Console.WriteLine(bsonDoc.ToJson());
        }
    }
}
