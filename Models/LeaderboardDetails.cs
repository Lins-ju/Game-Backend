using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class LeaderboardDetails
    {
        [JsonPropertyName("userName")]
        public string UserId { get; set; }

        [JsonPropertyName("carName")]
        public int CarId { get; set; }

        [JsonPropertyName("skinId")]
        public int SkinId { get; set; }

        [JsonPropertyName("timeStamp")]
        public long TimeStamp { get; set; }


        public LeaderboardDetails(string userId, int carId, int skinId)
        {
            this.UserId = userId;
            this.CarId = carId;
            this.SkinId = skinId;
            this.TimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}