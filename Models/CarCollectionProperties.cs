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
    }
}