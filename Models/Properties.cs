using Amazon.DynamoDBv2.DocumentModel;

namespace Backend.Models
{
    public class Properties
    {
        public string CarId { get; set; }
        public string SkinId { get; set; }
        public double Score { get; set; }
        public Properties(string carId, string skinId, double score)
        {
            CarId = carId;
            SkinId = skinId;
            Score = score;
        }
        public Properties()
        {

        }

        public static Document ToDocument(Properties properties)
        {
            var propertiesDocument = new Document
            {
                ["CarId"] = properties.CarId,
                ["SkinId"] = properties.SkinId,
                ["Score"] = properties.Score
            };

            return propertiesDocument;
        }

        public static Properties FromDocument(Document document)
        {
            var dynamoEntryDocument = document["Properties"].AsDocument();
            var carId = (string)dynamoEntryDocument["CarId"];
            var skinId = (string)dynamoEntryDocument["SkinId"];
            var score = (double)dynamoEntryDocument["Score"];

            return new Properties(carId, skinId, score);
        }
    }
}