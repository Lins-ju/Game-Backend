namespace Backend.Models
{
    public class GetLeaderboardResponse
    {
        public List<LeaderboardRecord> records { get; set; }

        public GetLeaderboardResponse()
        {
            records = new List<LeaderboardRecord>();
        }

        public void AddLeaderboardRecord(LeaderboardRecord leaderboardRecord)
        {
            records.Add(leaderboardRecord);
        }
    }
}