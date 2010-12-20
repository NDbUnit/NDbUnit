using EPocalipse.Json.Viewer;
using MongoDB.Bson;

namespace MongoDBCSharpDriverTest
{
    public class JsonToBson
    {
        public static BsonDocument ToBson(string json)
        {
            var root = new BsonDocument();

            var jsonTree = JsonObjectTree.Parse(json);

            enumerate(jsonTree.Root, root);

            return root;

        }

        private static void enumerate(JsonObject jsonObject, BsonDocument document)
        {

            var bsonDocument = document;


            foreach (JsonObject field in jsonObject.Fields)
            {

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

                        var array = getarray(field, null);
                        document.Add(field.Id, array);

                        break;
                }

                enumerate(field, bsonDocument);
            }
        }

        private static BsonArray getarray(JsonObject jsonObject, BsonArray array)
        {
            if (array == null)
                array = new BsonArray();

            foreach (JsonObject field in jsonObject.Fields)
            {

                switch (field.JsonType)
                {
                    case JsonType.Value:

                        if (array.Count > 0 && array[array.Count - 1].GetType() == typeof(BsonDocument))
                        {
                            ((BsonDocument)array[array.Count - 1]).Add(field.Id, BsonValue.Create(field.Value));
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
    }
}
