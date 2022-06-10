using Amazon.DynamoDBv2.DocumentModel;

namespace Backend.Models
{
    public class LeaderboardData
    {
        public string TrackId { get; set; }
        public string UserId { get; set; }
        public List<Properties> Properties { get; set; }

        public LeaderboardData(string trackId, string userId, List<Properties> properties)
        {
            TrackId = trackId;
            UserId = userId;
            Properties = properties;
        }

        public Document ToDocument()
        {
            var dynamoList = new DynamoDBList();
            foreach (var item in Properties)
            {
                dynamoList.Add(new Document
                {
                    ["CarId"] = item.CarId,
                    ["SkinId"] = item.SkinId,
                    ["Score"] = item.Score
                });
            }
            var document = new Document
            {
                ["TrackId"] = this.TrackId,
                ["OrderId"] = this.UserId,
                ["Properties"] = dynamoList
            };

            return document;
        }

        public static LeaderboardData FromDocument(Document document)
        {
            List<Properties> properties = new List<Properties>();
            var dynamoList = document["Properties"] as DynamoDBList;

            foreach (var objs in dynamoList.Entries)
            {
                var dynamoEntryDocument = objs.AsDocument();
                var carId = (int)dynamoEntryDocument["CarId"];
                var skinId = (int)dynamoEntryDocument["SkinId"];
                var score = (double)dynamoEntryDocument["Score"];

                properties.Add(new Properties(carId, skinId, score));
            }

            var Data = new LeaderboardData(document["TrackId"], document["UserId"], properties);
            return Data;
        }
    }
}