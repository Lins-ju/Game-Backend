
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2.DocumentModel;
using Backend.Models.S3;

namespace Backend.Models
{
    public class GameUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("carCollectionList")]
        public CarCollectionList CarCollectionList { get; set; }

        public GameUser()
        {
            
        }

        public GameUser(string id, string userName, CarCollectionList carCollectionList)
        {
            Id = id;
            UserName = userName;
            CarCollectionList = carCollectionList;
        }

        public static Document ToDocument(GameUser gameUser)
        {
            var carCollection = CarCollectionList.ListToDocument(gameUser.CarCollectionList);
            var document = new Document
            {
                ["Id"] = gameUser.Id,
                ["UserName"] = gameUser.UserName,
                ["CarCollectionList"] = carCollection
            };

            return document;
        }

        public static GameUser FromDocument(Document document)
        {
            var carCollectionList = CarCollectionList.FromDocumentToList(document["CarCollectionList"].AsListOfDocument());
            var id = (string)document["Id"];
            var userName = (string)document["UserName"];

            return new GameUser(id, userName, carCollectionList);
        }
    }
}