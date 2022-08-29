namespace Backend.Models.S3
{
    public class GetCarConfig : AbstractConfig
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public IFormFile CarImg { get; set; }

        public GetCarConfig(string carName, int maxSpeed, IFormFile carImg)
        {
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarImg = carImg;
        }

        public GetCarConfig(int id, string carName, int maxSpeed, CarType carType, IFormFile carImg)
        {
            Id = id;
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarType = carType;
            CarImg = carImg;
        }
        public GetCarConfig(CarConfig carConfig, IFormFile carImg)
        {
            Id = carConfig.Id;
            CarName = carConfig.CarName;
            MaxSpeed = carConfig.MaxSpeed;
            CarType = carConfig.CarType;
            CarImg = carImg;
        }

        public GetCarConfig(GetCarConfig cars)
        {
            CarName = cars.CarName;
            MaxSpeed = cars.MaxSpeed;
            CarType = cars.CarType;
            CarImg = cars.CarImg;
        }

        public GetCarConfig()
        {

        }
    }
}