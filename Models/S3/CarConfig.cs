namespace Backend.Models.S3
{
    public class CarConfig : AbstractConfig
    {
        public string CarId { get; set; }
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public string CarImgUrl { get; set; }
        public CarConfig(int id, string carName, int maxSpeed, CarType carType, string carImgUrl)
        {
            Type = "car";
            Id = id;
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarType = carType;
            CarImgUrl = carImgUrl;
        }

        public CarConfig()
        {

        }
    }
}