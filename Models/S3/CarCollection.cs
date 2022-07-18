namespace Backend.Models.S3
{
    public class CarCollection
    {
        public string CarName { get; set; }
        public int CarId { get; set; }
        public int MaxSpeed { get; set; }
        public CarType CarType { get; set; }
    }
}