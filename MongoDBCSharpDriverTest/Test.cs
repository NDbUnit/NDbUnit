using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using JsonParser;
using Newtonsoft.Json;
using NUnit.Framework;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;
using EPocalipse.Json.Viewer;
using System.Linq;


namespace MongoDBCSharpDriverTest
{
    [TestFixture]
    public class TestJsonParser
    {
        private string jsonText = null;
        Hashtable jsonObject = null;

        private string jsonText2 = null;
        Hashtable jsonObject2 = null;
        private int level;

        private BsonDocument root;

        private IList<BsonDocument> bsonDocuments;


        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            jsonText = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json.txt")).ReadToEnd();
            jsonObject = JSON.JsonDecode(jsonText) as Hashtable;

            jsonText2 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json2.txt")).ReadToEnd();
            jsonObject2 = JSON.JsonDecode(jsonText2) as Hashtable;

            bsonDocuments = new List<BsonDocument>();

            root = new BsonDocument();


        }

        BsonArray getarray(JsonObject jsonObject, BsonArray array)
        {
            if (array == null)
               array = new BsonArray();

            foreach (JsonObject field in jsonObject.Fields)
            {

                switch (field.JsonType)
                {
                    case JsonType.Value:
                        
                        if (array.Count > 0 && array[array.Count-1].GetType() ==  typeof(BsonDocument))
                        {
                            ((BsonDocument) array[array.Count - 1]).Add(field.Id, BsonValue.Create(field.Value));
                        }
                        else
                        {
                             array.Add(BsonValue.Create(field.Value));    
                        }
                        
                        break;
                    case JsonType.Object:
                        array.Add(new BsonDocument());
                        array = getarray(field, array);
                        break;
                    case JsonType.Array:
                        break;
                }

            }

            return array;

        }

        void enumerate(JsonObject jsonObject,BsonDocument document)
        {

            var bsonDocument = document;
            
            
            foreach (JsonObject field in jsonObject.Fields)
            {

                Console.WriteLine("Key: {0} Value: {1} Type: {2} ", field.Id, field.Value, field.JsonType);

                Console.WriteLine(root.ToJson());

                if (field.Parent.JsonType == JsonType.Array)
                    continue;

                switch (field.JsonType)
                {
                    case JsonType.Value:
                        bsonDocument.Add(field.Id, BsonValue.Create(field.Value));
                        break;
                    case JsonType.Object:
                        var newDocument = new BsonDocument();
                            bsonDocument.Add(field.Id, newDocument);
                            bsonDocument = newDocument;
                        break;
                    case JsonType.Array:


                        var array = getarray(field,null);
                        document.Add(field.Id, array);

                        break;
                        
                }

                enumerate(field,bsonDocument);
            }
        }

        [Test]
        public void bsonTest2()
        {
            BsonDocument root = new BsonDocument();

            root.Add("id", "0001");
            root.Add("type", "donut");
            root.Add("name", "Cake");
            root.Add("ppu", .55);

            BsonDocument bsonDocument0 = new BsonDocument();

            root.Add("batters", bsonDocument0);

            BsonArray bsonArray = new BsonArray();

            bsonDocument0.Add("batter", bsonArray);

            BsonDocument arrayBsonDocument = new BsonDocument();
  
            arrayBsonDocument.Add("id", "1001");
            arrayBsonDocument.Add("type", "Regular");
            
            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "1002");
            arrayBsonDocument.Add("type", "Chocolate");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "1003");
            arrayBsonDocument.Add("type", "Blueberry");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "1004");
            arrayBsonDocument.Add("type", "Devil's Food");

            bsonArray.Add(arrayBsonDocument);

            bsonArray = new BsonArray();

            bsonDocument0.Add("Topping", bsonArray);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5001");
            arrayBsonDocument.Add("type", "None");

            bsonArray.Add(arrayBsonDocument);


            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5002");
            arrayBsonDocument.Add("type", "Glazed");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5005");
            arrayBsonDocument.Add("type", "Surgar");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5007");
            arrayBsonDocument.Add("type", "Powdered Sugar");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5006");
            arrayBsonDocument.Add("type", "Chocolate with Sprinkles");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5003");
            arrayBsonDocument.Add("type", "Chocolate");

            bsonArray.Add(arrayBsonDocument);

            arrayBsonDocument = new BsonDocument();

            arrayBsonDocument.Add("id", "5004");
            arrayBsonDocument.Add("type", "Maple");

            bsonArray.Add(arrayBsonDocument);


            Console.WriteLine(root.ToJson());


        }

        [Test]
        public void bsonTest()
        {
            
            BsonDocument root = new BsonDocument();

            BsonDocument bsonDocument0 = new BsonDocument();

            root.Add("glossary", bsonDocument0);
 
            bsonDocument0.Add("title", "example glossary");

            var bsonDocument1 = new BsonDocument();
            
            
            bsonDocument0.Add("GlossDiv", bsonDocument1);

            bsonDocument1.Add("Title", "S");

            var bsonDocument2 = new BsonDocument();

            bsonDocument1.Add("GlossList", bsonDocument2);

            var bsonDocument3 = new BsonDocument();

            bsonDocument2.Add("GlossEntry", bsonDocument3);

            bsonDocument3.Add("ID", "SGML");
            bsonDocument3.Add("SortAs", "SGML");
            bsonDocument3.Add("GlossTerm", "Standard Generalerlized Markup Language");
            bsonDocument3.Add("Acronym", "SGML");
            bsonDocument3.Add("Abbrev", "ISO 8879:1986");

            var bsonDocument4 = new BsonDocument();
            bsonDocument3.Add("GlossDef", bsonDocument4);


            bsonDocument4.Add("para", "A meta-markup language used to create markup languages such as DocBook.");
            bsonDocument4.Add("GlossSeeAlso", new BsonArray() {"GML", "XML"});

            bsonDocument4.Add("GlossSee", "markup");

            Console.WriteLine(root.ToJson());

            var jsonTree = JsonObjectTree.Parse(jsonText2);
        }

        [Test]
        public void jsonViewerTest()
        {
            var jsonTree = JsonObjectTree.Parse(jsonText2);

            enumerate(jsonTree.Root,root);

            Console.WriteLine(root.ToJson());

            jsonTree = JsonObjectTree.Parse(jsonText);

            enumerate(jsonTree.Root, root);

            Console.WriteLine(root.ToJson());



        }

        [Test]
        public void CanVerifyID()
        {
            Assert.That("0001", Is.EqualTo(jsonObject["id"]));
        }

        [Test]
        public void CanVerifyType()
        {
            Assert.That("donut", Is.EqualTo(jsonObject["type"]));
        }

        [Test]
        public void CanVerifyPPU()
        {
            Assert.That(.55, Is.EqualTo(jsonObject["ppu"]));
        }

        [Test]
        public void CanDeterminePPUDataType()
        {

            Type actualType = jsonObject["ppu"].GetType();
            Type expectedType = .55D.GetType();
            Assert.That(expectedType, Is.EqualTo(actualType));
        }

        [Test]
        public void CanVerifyToppingCount()
        {
            var topping = jsonObject["topping"] as ArrayList;

            Assert.That(7, Is.EqualTo(topping.Count));
        }

        [Test, Ignore]
        public void CanCreateConnection()
        {
            string connectionString = "mongodb://localhost";
            MongoServer server = MongoServer.Create(connectionString);

            server.Connect(System.TimeSpan.FromSeconds(30));
            Assert.That(server.State, Is.EqualTo(MongoServerState.Connected));
        }

        [Test, Ignore]
        public void ClearCollection()
        {

            MongoServer server = MongoServer.Create("mongodb://localhost");
            server.Connect();
            server.GetDatabase("books").GetCollection("books").RemoveAll();


        }

        [Test, Ignore]
        public void CreateDB()
        {
            MongoServer server = MongoServer.Create("mongodb://localhost");
            server.Connect();

            var db = server.GetDatabase("books");

            BsonDocument book = new BsonDocument() 
            {
                { "Author", "Ernest Hemingway" },
                { "Title", "For Whom the Bell Tolls" },
                {"PublicationDate",1940}
            };

            MongoCollection<BsonDocument> books = new MongoCollection<BsonDocument>(db, "books");

            books.Save(book);

            book = new BsonDocument() 
            {
                { "Author", "John Steinbeck" },
                { "Title", "The Grapes of Wrath" },
                {"PublicationDate",1939}
            };

            books.Save(book);
        }
        [Test, Ignore]
        public void CanCreateDocument()
        {

            /*
            BsonElement author = new BsonElement("Author", "");
            BsonElement title = new BsonElement("Title", "");
            BsonElement publishdate = new BsonElement("Title",0);


            IList<BsonElement> elements = new List<BsonElement>();

            author.Value = BsonValue.Create("Ernest Hemingway");
            title.Value = BsonValue.Create("For Whom the Bell Tolls");
            publishdate.Value = BsonValue.Create(true);
            
            elements.Add(author);

            

            BsonDocument book = new BsonDocument() 
            {
                elements
            };
             */

            MongoServer server = MongoServer.Create("mongodb://localhost");
            server.Connect();

            var db = server.GetDatabase("books");
            var books = db.GetCollection("books");






            var x = db.GetCollection("books").FindAll();


        }

        [Test, Ignore]
        public void CanCreateDocumentArray()
        {



            BsonArray array = new BsonArray() 
            { 
               new BsonDocument()
                {
                 { "author", "Ernest Hemingway" },
                 { "title", "For Whom the Bell Tolls" }
                },

               new BsonDocument()
                {
                 { "author", "John Steinbeck" },
                 { "title", "The Grapes of Wrath" }
                }
            };

            Assert.AreEqual(2, array.Count);


        }

        [Test, Ignore]
        public void foo()
        {
            BsonDocument document = new BsonDocument();
            string name = document["name"].AsString;
            int age = document["age"].AsInt32;
            BsonDocument address = document["address"].AsBsonDocument;
            string zip = address["zip"].AsString;


        }
    }

}