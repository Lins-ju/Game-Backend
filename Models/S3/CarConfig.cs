using System.Text.Json.Serialization;
namespace Backend.Models.S3
{
    public class CarConfig : AbstractConfig
    {
        [JsonPropertyName("carName")]
        public string CarName { get; set; }
        [JsonPropertyName("maxSpeed")]
        public int MaxSpeed { get; set; }
        [JsonPropertyName("carType")]
        public CarType CarType { get; set; }
        [JsonPropertyName("skinId")]
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