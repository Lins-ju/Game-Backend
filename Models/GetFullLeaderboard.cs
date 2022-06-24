namespace Backend.Models
{
    public class GetFullLeaderboard
    {
        public List<LeaderboardDetail> FullLeaderboard = new List<LeaderboardDetail>();

        public GetFullLeaderboard()
        {
            FullLeaderboard = new List<LeaderboardDetail>();
        }

        public void AddLeaderboardDetail(List<LeaderboardDetail> leaderboardDataNewList)
        {
            FullLeaderboard = leaderboardDataNewList;
        }

        public void AddLeaderboardDetail(LeaderboardDetail leaderboardDetail)
        {
            FullLeaderboard.Add(leaderboardDetail);
        }
    }
}