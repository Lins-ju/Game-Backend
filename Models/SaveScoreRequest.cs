using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class SaveScoreRequest
    {
        //Leaderboard Data

        [JsonPropertyName("trackName")] 
        public string TrackId { get; set; }

        [JsonPropertyName("userId")]   
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        //Player Data

        [JsonPropertyName("carId")]
        public int CarId { get; set; }

        [JsonPropertyName("SkinId")]
        public int SkinId { get; set; }

        public SaveScoreRequest(string trackId, string userId, double score, int carId, int skinId)
        {
            this.TrackId = trackId;
            this.UserId = userId;
            this.Score = score;
            this.SkinId = skinId;
        }
    }
}