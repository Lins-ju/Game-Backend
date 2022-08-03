using Amazon.DynamoDBv2.DocumentModel;

namespace Backend.Models
{
    public class LeaderboardData
    {
        public string TrackId { get; set; }
        public string UserId { get; set; }
        public Properties Properties { get; set; }

        public LeaderboardData(string trackId, string userId, Properties properties)
        {
            TrackId = trackId;
            UserId = userId;
            Properties = properties;
        }

        public LeaderboardData()
        {
            
        }

        public Document ToDocument()
        {

            var propertiesDocument = new Document
            {
                ["CarId"] = this.Properties.CarId,
                ["SkinId"] = this.Properties.SkinId,
                ["Score"] = this.Properties.Score
            };
            var document = new Document
            {
                ["TrackId"] = this.TrackId,
                ["UserId"] = this.UserId,
                ["Properties"] = propertiesDocument
            };

            return document;
        }

        public static LeaderboardData FromDocument(Document document)
        {
            var dynamoEntryDocument = document["Properties"].AsDocument();
            var carId = (int)dynamoEntryDocument["CarId"];
            var skinId = (int)dynamoEntryDocument["SkinId"];
            var score = (double)dynamoEntryDocument["Score"];

            Properties properties = new Properties(carId, skinId, score);


            var Data = new LeaderboardData(document["TrackId"], document["UserId"], properties);
            return Data;
        }
    }
}