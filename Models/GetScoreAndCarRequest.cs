using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Backend.Models
{
    public class GetScoreAndCarRequest
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("carId")]
        public int CarId { get; set; }

        [JsonPropertyName("skinId")]
        public int SkinId { get; set; }

        public GetScoreAndCarRequest(LeaderboardEntry leaderboards, Cars cars)
        {
            this.UserName = leaderboards.UserId;
            this.Score = leaderboards.Score;
            this.CarId = cars.CarId;
            this.SkinId = cars.SkinId;
        }
    }
}