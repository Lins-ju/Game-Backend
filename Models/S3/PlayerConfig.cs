using Backend.Models.S3;

namespace Backend.Models
{
    public class PlayerConfig : AbstractConfig
    {
        public string UserImgUrl { get; set; }

        public PlayerConfig(int id, string userImgUrl)
        {
            Type = "player";
            Id = id;
            UserImgUrl = userImgUrl;
        }
    }
}