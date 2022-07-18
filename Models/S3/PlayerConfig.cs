namespace Backend.Models
{
    public class PlayerConfig
    {
        public int CarId { get; set; }
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