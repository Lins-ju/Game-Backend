using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Cars
    {

        [JsonPropertyName("carName")]
        public string CarName { get; set; }

        [JsonPropertyName("maxSpeed")]
        public int MaxSpeed { get; set; }

        [JsonPropertyName("skinId")]
        public int SkinId { get; set; }

        public Cars(string carName, int maxSpeed, int skinId)
        {
            this.CarName = carName;
            this.MaxSpeed = maxSpeed;
            this.SkinId = skinId;
        }
    }
}