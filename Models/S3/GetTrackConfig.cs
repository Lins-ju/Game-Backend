namespace Backend.Models.S3
{
    public class GetTrackConfigResponse
    {
        public string TrackId { get; set; }

        public GetTrackConfigResponse(string trackId)
        {
            TrackId = trackId;
        }
        public GetTrackConfigResponse()
        {

        }
    }
}