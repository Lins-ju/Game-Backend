using Backend.Models.S3;

namespace Backend.Models
{
    public class PlayerConfig : AbstractConfig
    {
        public int SkinId { get; set; }

        public PlayerConfig(int carId, int skinId)
        {
            CarId = carId;
            SkinId = skinId;
        }
        public PlayerConfig()
        {

        }
    }
}