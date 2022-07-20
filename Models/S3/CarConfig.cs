namespace Backend.Models.S3
{
    public class CarConfig
    {
        public int CarId { get; set; }
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }

        public CarConfig(int carId, string carName, int maxSpeed, CarType carType)
        {
            CarId = carId;
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarType = carType;
        }

        public CarConfig()
        {

        }
    }
}