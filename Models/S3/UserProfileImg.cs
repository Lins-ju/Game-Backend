using Backend.Models.S3;

namespace Backend.Models
{
    public class UserProfileImg : AbstractConfig
    {
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