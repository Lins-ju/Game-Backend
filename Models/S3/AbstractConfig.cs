using System.Text.Json.Serialization;

namespace Backend.Models.S3
{
    public abstract class AbstractConfig
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}