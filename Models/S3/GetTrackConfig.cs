namespace Backend.Models.S3
{
    public class GetTrackConfigResponse
    {
        public int TrackId { get; set; }

        public GetTrackConfigResponse(int trackId)
        {
            TrackId = trackId;
        }
        public GetTrackConfigResponse()
        {

        }
    }
}