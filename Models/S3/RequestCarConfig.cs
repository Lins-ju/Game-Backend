namespace Backend.Models.S3
{
    public class RequestCarConfig : AbstractConfig
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public IFormFile CarImg { get; set; }

        public RequestCarConfig(string carName, int maxSpeed, IFormFile carImg)
        {
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarImg = carImg;
        }

        public RequestCarConfig(int id, string carName, int maxSpeed, CarType carType, IFormFile carImg)
        {
            Id = id;
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarType = carType;
            CarImg = carImg;
        }
        public RequestCarConfig(CarConfig carConfig, IFormFile carImg)
        {
            Id = carConfig.Id;
            CarName = carConfig.CarName;
            MaxSpeed = carConfig.MaxSpeed;
            CarType = carConfig.CarType;
            CarImg = carImg;
        }

        public RequestCarConfig(RequestCarConfig carConfig)
        {
            CarName = carConfig.CarName;
            MaxSpeed = carConfig.MaxSpeed;
            CarType = carConfig.CarType;
            CarImg = carConfig.CarImg;
        }

        public RequestCarConfig()
        {

        }
    }
}