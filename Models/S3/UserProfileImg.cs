using Backend.Models.S3;
using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class UserProfileImg : AbstractConfig
    {
        [JsonPropertyName("userImgUrl")]
        public string UserImgUrl { get; set; }

        public UserProfileImg(string id, string userImgUrl)
        {
            Type = "player";
            Id = id;
            UserImgUrl = userImgUrl;
        }

        public UserProfileImg()
        {
            
        }
    }
}