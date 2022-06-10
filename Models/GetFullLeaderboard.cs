namespace Backend.Models
{
    public class GetFullLeaderboard
    {
        public List<LeaderboardDetail> leaderboardDetails = new List<LeaderboardDetail>();

        public GetFullLeaderboard()
        {
            leaderboardDetails = new List<LeaderboardDetail>();
        }

        public void AddLeaderboardDetail(LeaderboardDetail leaderboardDetail)
        {
            leaderboardDetails.Add(leaderboardDetail);
        }
    }
}