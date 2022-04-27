using System.Text.Json.Serialization;

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



        public GetScoreAndCarRequest(string userName, double score, int carId, int skinId)
        {
            this.UserName = userName;
            this.Score = score;
            this.CarId = carId;
            this.SkinId = skinId;
        }

        public GetScoreAndCarRequest(Leaderboard leaderboard, Cars cars)
        {
            this.UserName = leaderboard.Leaderboards[0].UserId;
            this.Score = leaderboard.Leaderboards[0].Score;
            this.CarId = cars.CarId;
            this.SkinId = cars.SkinId;
        }
    }
}