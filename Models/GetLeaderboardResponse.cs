using StackExchange.Redis;

namespace Backend.Models
{
    public class LeaderboardList
    {
        public List<GetScoreAndCarRequest> GetLeaderboardResponse { get; set; }

        public LeaderboardList(Leaderboard leaderboard, Cars cars)
        {
            GetLeaderboardResponse = new List<GetScoreAndCarRequest>();
            foreach(var item in leaderboard.Leaderboards)
            {
                GetLeaderboardResponse.Add(new GetScoreAndCarRequest(item, cars));
            }
        }
    }
}