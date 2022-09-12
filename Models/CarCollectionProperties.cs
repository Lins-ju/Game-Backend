using Amazon.DynamoDBv2.DocumentModel;
using Backend.Models.S3;

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

        public CarCollectionProperties(CarConfig car)
        {
            CarId = car.Id;
            SkinId = car.SkinId;
        }

        public CarCollectionProperties(RequestCarConfig car)
        {
            CarId = car.Id;
        }
    }
}