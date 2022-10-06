using System.Text.Json.Serialization;

namespace Backend.Models
{
    public class GetLeaderboardRequest
    {
        [JsonPropertyName("trackId")]
        public string TrackId { get; set; }

        [JsonConstructor]
        public GetLeaderboardRequest() 
        {

        }
        public GetLeaderboardRequest(string trackId)
        {
            this.TrackId = trackId;
        }

    }
}