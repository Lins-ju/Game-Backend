namespace backend.Models
{
    public class AddLeaderboardRequest
    {
        public string track { get; set; }
        public string name { get; set; }
        public long time { get; set; }

        public AddLeaderboardRequest()
        {
            
        }
    }
}