using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class LeaderboardDetail
    {
        [JsonPropertyName("userName")]
        public string UserId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("carId")]
        public int CarId { get; set; }

        [JsonPropertyName("skinId")]
        public int SkinId { get; set; }

        public LeaderboardDetail(string userId, int carId, int skinId, double score)
        {
            UserId = userId;
            CarId = carId;
            SkinId = skinId;
            Score = score;
        }

        public LeaderboardDetail(LeaderboardData leaderboardData)
        {
            UserId = leaderboardData.UserId;
            Score = leaderboardData.Properties.Score;
            CarId = leaderboardData.Properties.CarId;
            SkinId = leaderboardData.Properties.SkinId;
        }

        public LeaderboardDetail(LeaderboardData leaderboardData, LeaderboardEntry leaderboardEntry)
        {
            UserId = leaderboardEntry.UserId;
            Score = leaderboardEntry.Score;
            CarId = leaderboardData.Properties.CarId;
            SkinId = leaderboardData.Properties.SkinId;
        }

        public LeaderboardDetail()
        {

        }
    }
}