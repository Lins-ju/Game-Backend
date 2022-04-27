using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class SaveScoreRequest
    {
        //Leaderboard Data

        [JsonPropertyName("trackName")] 
        public string TrackName { get; set; }

        [JsonPropertyName("userId")]   
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        //Player Data

        [JsonPropertyName("carId")]
        public int CarId { get; set; }

        [JsonPropertyName("SkinId")]
        public int SkinId { get; set; }

        public SaveScoreRequest()
        {
            
        }
    }
}