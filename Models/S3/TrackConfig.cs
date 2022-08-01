namespace Backend.Models.S3
{
    public class TrackConfig
    {
        public string TrackId { get; set; }
        public string TrackImgId { get; set; }

        public TrackConfig(string trackId, string trackImgId)
        {
            TrackId = trackId;
            TrackImgId = trackImgId;
        }
        public TrackConfig()
        {

        }
    }
}