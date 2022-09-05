namespace Backend.Models.S3
{
    public class TrackConfig : AbstractConfig
    {
        public string TrackName { get; set; }
        public TrackConfig(int id, string trackName)
        {
            Type = "track";
            Id = id;
            TrackName = trackName;
        }
        public TrackConfig()
        {
            
        }
    }
}