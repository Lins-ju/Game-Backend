namespace Backend.Models.S3
{
    public class CarConfig : AbstractConfig
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public int SkinId { get; set; }
        public CarConfig(int id, string carName, int maxSpeed, CarType carType, int skinId)
        {
            Type = "car";
            Id = id;
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarType = carType;
            SkinId = skinId;
        }

        public CarConfig()
        {

        }
    }
}