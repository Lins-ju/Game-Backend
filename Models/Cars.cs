using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class Cars
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("carName")]
        public int CarId { get; set; }

        [JsonPropertyName("skinId")]
        public int SkinId { get; set; }

        public Cars(string userName, int carId, int skinId)
        {
            this.UserName = userName;
            this.CarId = carId;
            this.SkinId = skinId;
        }
    }
}