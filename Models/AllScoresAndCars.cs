using StackExchange.Redis;

namespace Backend.Models
{
    public class AllScoresAndCarsResponse
    {
        public List<GetScoreAndCarRequest> ScoreAndCarRequests;

        public AllScoresAndCarsResponse(Leaderboard leaderboard, Cars cars)
        {
            ScoreAndCarRequests = new List<GetScoreAndCarRequest>();

            foreach(var item in leaderboard.Leaderboards)
            {
                ScoreAndCarRequests.Add(new GetScoreAndCarRequest(item, cars));
            }
        }
    }
}