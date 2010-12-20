
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
            jsonText = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json.txt")).ReadToEnd();
            jsonText2 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json2.txt")).ReadToEnd();
            jsonText3 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json3.txt")).ReadToEnd();
        
        }


        [Test]
        public void CanConvertJsonToBson()
        {
            var bsonDoc = JsonToBson.ToBson(jsonText2);

            Console.WriteLine(bsonDoc.ToJson());
        }
    }
}
