using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class SaverCarRequest
    {
        [JsonPropertyName("carName")]
        public string CarName { get; set; }

        [JsonPropertyName("maxSpeed")]
        public int MaxSpeed { get; set; }

        [JsonPropertyName("skinId")]
        public int SkinId { get; set; }
    }
}