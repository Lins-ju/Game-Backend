using Backend.Models.S3;

namespace Backend.Models
{
    public class SaveCarRequest
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public string CarSkinImg { get; set; }
        public int CarType { get; set; }

        public SaveCarRequest(string carName, int maxSpeed, string carSkinImg, int carType)
        {
            CarName = carName;
            MaxSpeed = maxSpeed;
            CarSkinImg = carSkinImg;
            CarType = carType; 
        }
        public SaveCarRequest()
        {
        }
    }
}