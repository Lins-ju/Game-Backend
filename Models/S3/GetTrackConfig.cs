namespace Backend.Models.S3
{
    public class GetTrackConfig
    {
        public string TrackId { get; set; }
        public IFormFile ImgFile { get; set; }

        public GetTrackConfig(string trackId, IFormFile imgFile)
        {
            TrackId = trackId;
            ImgFile = imgFile;
        }
        public GetTrackConfig()
        {
            
        }
    }
}