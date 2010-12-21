using System;
using System.IO;
using System.Reflection;
using MongoDB.Bson;
using NUnit.Framework;

namespace MongoDBCSharpDriverTest
{
    [TestFixture]
    public class jsonParsertest
    {
        [Test]
        public void ExtensionMethodTest()
        {
            var jsonText1 = new StreamReader(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json.txt")).
                        ReadToEnd();

            var jsonText2 = new StreamReader(
                        Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json2.txt")).
                        ReadToEnd();


            var bsonDocument1 = new BsonDocument().FromJson(jsonText1);
                    
            var bsonDocument2 = new BsonDocument().FromJson(jsonText2);

         
            Console.WriteLine("Generated Json: bsonDocument1:");
            Console.WriteLine(bsonDocument1.ToJson());
            Console.WriteLine("Raw File: jsonText1:");
            Console.WriteLine(jsonText1);

            Console.WriteLine("Generated Json: bsonDocument2:");
            Console.WriteLine(bsonDocument2.ToJson());
            Console.WriteLine("Raw File: jsonText2:");
            Console.WriteLine(jsonText2);

            Assert.AreEqual(jsonText1.Length,bsonDocument1.ToJson().Length);
            Assert.AreEqual(jsonText2.Length,bsonDocument2.ToJson().Length);
        }
    }
}