namespace backend.Models
{
    public class AddLeaderboardRequest
    {
        public string Track { get; set; }
        public string Name { get; set; }
        public long Time { get; set; }

        public AddLeaderboardRequest()
        {
            
        }
    }
}