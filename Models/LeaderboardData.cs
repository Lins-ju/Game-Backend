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

        public static Document ToDocument(LeaderboardData leaderboardData)
        {
            var propertiesDocument = Properties.ToDocument(leaderboardData.Properties);
            var document = new Document
            {
                ["TrackId"] = leaderboardData.TrackId,
                ["UserId"] = leaderboardData.UserId,
                ["Properties"] = propertiesDocument
            };

            return document;
        }

        public static LeaderboardData FromDocument(Document document)
        {
            var properties = Properties.FromDocument(document);
            var Data = new LeaderboardData(document["TrackId"], document["UserId"], properties);
            return Data;
        }
    }
}