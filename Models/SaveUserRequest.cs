using System.Text.Json.Serialization;
using Backend.Models.S3;

namespace Backend.Models
{
    public class SaveUserRequest
    {
        public string UserName { get; set; }
        public string UserProfileImg { get; set; }
        public List<CarCollectionProperties> CarCollectionList { get; set; }

        [JsonConstructor]
        public SaveUserRequest()
        {    
        }
        public SaveUserRequest(string userName, string userProfileImg, List<CarCollectionProperties> carCollection)
        {
            UserName = userName;
            UserProfileImg = userProfileImg; 
            CarCollectionList = carCollection; 
        }
    }
}