using Amazon.DynamoDBv2.DocumentModel;
using Backend.Models.S3;

namespace Backend.Models
{
    public class CarCollectionProperties
    {
        public string CarId { get; set; }
        public string SkinId { get; set; }

        public CarCollectionProperties(string carId, string skinId)
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