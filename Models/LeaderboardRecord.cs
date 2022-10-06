using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Backend.Models
{
    public class LeaderboardRecord
    {
        [JsonPropertyName("userName")]
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("carId")]
        public string CarId { get; set; }

        [JsonPropertyName("skinId")]
        public string SkinId { get; set; }

        [JsonPropertyName("timeStamp")]
        public long TimeStamp { get; set; }

        public LeaderboardRecord(LeaderboardEntry leaderboards, LeaderboardDetails leaderboardDetails)
        {
            this.UserId = leaderboards.UserId;
            this.Score = leaderboards.Score;
            this.CarId = leaderboardDetails.CarId;
            this.SkinId = leaderboardDetails.SkinId;
            this.TimeStamp = leaderboardDetails.TimeStamp;
        }
    }
}