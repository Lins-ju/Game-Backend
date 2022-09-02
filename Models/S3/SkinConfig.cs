namespace Backend.Models.S3
{
    public class SkinConfig : AbstractConfig
    {
        public string SkinImgUrl { get; set; }

        public SkinConfig(int id, string skinImgUrl)
        {
            Type = "skin";
            Id = id;
            SkinImgUrl = skinImgUrl; 
        }
        public SkinConfig()
        {

        }
    }
}