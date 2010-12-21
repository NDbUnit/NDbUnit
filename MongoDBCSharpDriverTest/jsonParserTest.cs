
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MongoDB.Bson;
using NUnit.Framework;


public enum JsonObjectType
{
    HashTable,
    ArrayList,
    Value
}




[TestFixture]
public class jsonParsertest
{
    private string jsonText;
    private JsonObjectType jsonType;
    private BsonDocument root;
    

    [TestFixtureSetUp]
    void Setup()
    {
        jsonText = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json.txt")).ReadToEnd();
        root = new BsonDocument();
    }

    [Test]
    public void TestParser()
    {
        
        var json = JsonParser.JSON.JsonDecode(jsonText) as Hashtable;
        enumerate(json,root,null);

        Console.WriteLine(root.ToJson());

    }


    void enumerate(Object jsonObject, BsonDocument document, BsonArray array)
    {
        var hashtable = jsonObject as Hashtable;

        if (hashtable != null)
        {
           if (array != null)
           {
               var newDocument = new BsonDocument();
               array.Add(newDocument);
           }

            foreach (DictionaryEntry dictionaryEntry in hashtable)
            {
                
                if (dictionaryEntry.Value.GetType() == typeof(Hashtable))
               {
                    var newDocument = new BsonDocument();
                    if (array != null)
                    {
                        array.Add(newDocument);
                        newDocument.Add(dictionaryEntry.Key.ToString(), BsonValue.Create(dictionaryEntry.Value));
                    }
                    else
                    {
                       document.Add(dictionaryEntry.Key.ToString(), newDocument);    
                    }
                    
                    enumerate(dictionaryEntry.Value, newDocument,array);
               }
               else
               {
                   if (dictionaryEntry.Value.GetType() == typeof(ArrayList))
                   {

                       var newArray = new BsonArray();
                       document.Add(dictionaryEntry.Key.ToString(), newArray);
                       
                       foreach (object entry in dictionaryEntry.Value as ArrayList)
                       {
                           enumerate(entry,document,newArray);
                       }
                   }
                   else
                   {
                       if (array != null)
                       {
                           if (array.Count > 0 && array[array.Count-1].GetType() == typeof(BsonDocument))
                           {
                               ((BsonDocument) array[array.Count - 1]).Add(dictionaryEntry.Key.ToString(), BsonValue.Create(dictionaryEntry.Value));
                           }
                           else
                           {
                               array.Add(BsonValue.Create(dictionaryEntry.Value));
                           }
                       }
                       else
                       {
                           document.Add(dictionaryEntry.Key.ToString(), BsonValue.Create(dictionaryEntry.Value));
                       }
                       Console.WriteLine("Key:{0} Value:{1}", dictionaryEntry.Key, dictionaryEntry.Value);     
                   }
               }    
            }
        }
        else
        {
            if (array != null)
            {
                array.Add(BsonValue.Create(jsonObject));
            }
        }
   }
}