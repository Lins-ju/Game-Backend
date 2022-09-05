using Backend.Models.S3;

namespace Backend.Models
{
    public class SaveCarRequest
    {
        public string CarName { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
        public IFormFile CarSkinImg { get; set; }
    }
}