using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class SaveScoreRequest
    {
        //Leaderboard Data

        [JsonPropertyName("trackId")] 
        public string TrackId { get; set; }

        [JsonPropertyName("userId")]   
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        //Player Data

        [JsonPropertyName("carId")]
        public string CarId { get; set; }

        [JsonPropertyName("skinId")]
        public string SkinId { get; set; }

        public SaveScoreRequest(string trackId, string userId, double score, string carId, string skinId)
        {
            this.TrackId = trackId;
            this.UserId = userId;
            this.Score = score;
            this.SkinId = skinId;
        }
    }
}