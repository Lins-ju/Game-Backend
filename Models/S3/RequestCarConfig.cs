namespace Backend.Models.S3
{
    public class RequestCarConfig : AbstractConfig
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public string CarImg { get; set; }
        public string SkinId { get; set; }

        public RequestCarConfig(string carName, int maxSpeed, string carImg)
        {
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarImg = carImg;
        }

        public RequestCarConfig(string id, string carName, int maxSpeed, CarType carType, string carImg, string skinId)
        {
            Id = id;
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarType = carType;
            CarImg = carImg;
            SkinId = skinId;
        }
        public RequestCarConfig(CarConfig carConfig, string carImg)
        {
            Id = carConfig.Id;
            CarName = carConfig.CarName;
            MaxSpeed = carConfig.MaxSpeed;
            CarType = carConfig.CarType;
            CarImg = carImg;
            SkinId = carConfig.SkinId;
        }

        public RequestCarConfig(RequestCarConfig carConfig)
        {
            CarName = carConfig.CarName;
            MaxSpeed = carConfig.MaxSpeed;
            CarType = carConfig.CarType;
            CarImg = carConfig.CarImg;
            SkinId = carConfig.SkinId;
        }

        public RequestCarConfig()
        {

        }
    }
}