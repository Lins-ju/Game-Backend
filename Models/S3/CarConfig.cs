namespace Backend.Models.S3
{
    public class CarConfig : AbstractConfig
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public string SkinId { get; set; }
        public CarConfig(string id, string carName, int maxSpeed, CarType carType, string skinId)
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