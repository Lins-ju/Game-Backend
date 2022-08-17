using Amazon.DynamoDBv2.DocumentModel;

namespace Backend.Models
{
    public class Properties
    {
        public int CarId { get; set; }
        public int SkinId { get; set; }
        public double Score { get; set; }

        public Properties(int carId, int skinId, double score)
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
            var carId = (int)dynamoEntryDocument["CarId"];
            var skinId = (int)dynamoEntryDocument["SkinId"];
            var score = (double)dynamoEntryDocument["Score"];

            Properties properties = new Properties(carId, skinId, score);

            return properties;
        }
    }
}