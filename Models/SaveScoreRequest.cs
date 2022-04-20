using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class SaveScoreRequest
    {
        [JsonPropertyName("trackName")] 
        public string TrackName { get; set; }

        [JsonPropertyName("userId")]   
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        public SaveScoreRequest()
        {
            
        }
    }
}