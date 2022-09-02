using Backend.Models.S3;

namespace Backend.Models
{
    public class SaveUserRequest
    {
        public string UserName { get; set; }
        public CarCollectionList CarCollectionList { get; set; }
        public IFormFile UserImg { get; set; }

        public SaveUserRequest(string userName, CarCollectionList carCollection, IFormFile userImg)
        {
            UserName = userName;
            CarCollectionList = carCollection;
            UserImg = userImg;
        }
    }
}