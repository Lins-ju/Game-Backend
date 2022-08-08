namespace Backend.Models.S3
{
    public class TrackConfig
    {
        public string TrackId { get; set; }
        public string TrackImgUrl { get; set; }

        public TrackConfig(string trackId, string trackImgUrl)
        {
            TrackId = trackId;
            TrackImgUrl = trackImgUrl;
        }
        public TrackConfig()
        {
            
        }
    }
}