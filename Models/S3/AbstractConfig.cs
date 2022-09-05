using System.Text.Json.Serialization;

namespace Backend.Models.S3
{
    public abstract class AbstractConfig
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}