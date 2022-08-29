using Amazon.DynamoDBv2.DocumentModel;

namespace Backend.Models
{
    public class CarCollectionProperties
    {
        public int CarId { get; set; }
        public int SkinId { get; set; }

        public CarCollectionProperties(int carId, int skinId)
        {
            CarId = carId;
            SkinId = skinId;
        }

        public static Document ToDocument(CarCollectionProperties carCollectionProperties)
        {
            var carCollectionPropertiesDocument = new Document
            {
                ["CarId"] = carCollectionProperties.CarId,
                ["SkinId"] = carCollectionProperties.SkinId,
            };

            return carCollectionPropertiesDocument;
        }

        public static CarCollectionProperties FromDocument(Document document)
        {
            var dynamoEntryDocument = document["CarCollectionProperties"].AsDocument();
            var carId = (int)dynamoEntryDocument["CarId"];
            var skinId = (int)dynamoEntryDocument["SkinId"];

            return new CarCollectionProperties(carId, skinId);
        }
    }
}