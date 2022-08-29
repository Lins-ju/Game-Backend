namespace Backend.Models.S3
{
    public class SkinConfig : AbstractConfig
    {
        public string SkinImgUrl { get; set; }
        public CarPaintType CarType { get; set; }

        public SkinConfig(int id, CarPaintType carType)
        {
            Type = "skin";
            Id = id;
            CarType = carType;
        }
        public SkinConfig()
        {

        }
    }
}