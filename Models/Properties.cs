namespace Backend.Models
{
    public class Properties
    {
        public int CarId { get; set; }
        public int SkinId { get; set; }
        public double Score { get; set; }

        public Properties(int carId, int skinId, double score)
        {
            CarId = carId;
            SkinId = skinId;
            Score = score;
        }
    }
}