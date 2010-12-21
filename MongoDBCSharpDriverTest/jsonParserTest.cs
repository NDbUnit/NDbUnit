
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
    
    [TestFixtureSetUp]
    void Setup()
    {
        jsonText = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("MongoDBCSharpDriverTest.json2.txt")).ReadToEnd();

    }

    [Test]
    public void TestParser()
    {
        var root = new BsonDocument();
        
        var json = JsonParser.JSON.JsonDecode(jsonText) as Hashtable;
        enumerate(json,root);

        Console.WriteLine(root.ToJson());

    }


    void enumerate(Object jsonObject, BsonDocument document)
    {

        var hashtable = jsonObject as Hashtable;

        if (hashtable != null)
        {
            foreach (DictionaryEntry dictionaryEntry in hashtable)
            {
               if (dictionaryEntry.Value.GetType() == typeof(Hashtable))
               {
                   enumerate(dictionaryEntry.Value, document);
               }
               else
               {
                   if (dictionaryEntry.Value.GetType() == typeof(ArrayList))
                   {
                       foreach (object entry in dictionaryEntry.Value as ArrayList)
                       {
                           enumerate(entry,document);
                       }
                   }
                   else
                   {
                       Console.WriteLine("Key:{0} Value:{1}", dictionaryEntry.Key, dictionaryEntry.Value);     
                   }
               }    
            }
        }
        else
        {
            var arraylist = jsonObject as ArrayList;
            if (arraylist != null)
            {
                foreach (object entry in arraylist)
                {
                    enumerate(entry,document);
                }
            }
            else
            {
               Console.WriteLine(jsonObject);
            }
        }
    }

    private static BsonArray getarray(ArrayList jsonObject, BsonArray array)
    {
        if (array == null)
            array = new BsonArray();

        JsonObjectType jsonType = JsonObjectType.Value;

        return array;
        /*
        
        foreach (DictionaryEntry dictionaryEntry in jsonObject)
        {

            if (dictionaryEntry.Key.GetType() == typeof(Hashtable))
            {
                jsonType = JsonObjectType.HashTable;
                array.Add(new BsonDocument());
                array = getarray(dictionaryEntry.Value as Hashtable, array);
            }

            if (dictionaryEntry.Key.GetType() == typeof(ArrayList))
            {
                jsonType = JsonObjectType.ArrayList;
                array.Add(new BsonArray());
                array = getarray(dictionaryEntry.Value as Hashtable, array);
            }

            if (jsonType == JsonObjectType.Value)
            {
                if (array.Count > 0 && array[array.Count - 1].GetType() == typeof(BsonDocument))
                {
                    ((BsonDocument)array[array.Count - 1]).Add(dictionaryEntry.Key.ToString(), BsonValue.Create(dictionaryEntry.Value));
                }
                else
                {
                    if (array.Count > 0 && array[array.Count - 1].GetType() == typeof(BsonArray))
                    {
                        ((BsonArray)array[array.Count - 1]).Add(BsonValue.Create(dictionaryEntry.Value));
                    }
                    else
                    {
                        array.Add(BsonValue.Create(dictionaryEntry.Value));
                    }
                }
            }
         

         }

          return array;
         */
    }

}