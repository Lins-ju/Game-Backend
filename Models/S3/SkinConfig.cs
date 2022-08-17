namespace Backend.Models.S3
{
    public class SkinConfig
    {
        public string SkinColor { get; set; }
        public CarPaintType CarType { get; set; }

        public SkinConfig(string skinColor, CarPaintType carType)
        {
            SkinColor = skinColor;
            CarType = carType;
        }
        public SkinConfig()
        {

        }
    }
}