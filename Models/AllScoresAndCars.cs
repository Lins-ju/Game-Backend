namespace Backend.Models
{

    // IN DEVELOPMENT, SO NOT IN USE
    public class AllScoresAndCars
    {
        public List<GetScoreAndCarRequest> ScoreAndCarRequests = new List<GetScoreAndCarRequest>();

        public AllScoresAndCars(Leaderboard entries, Cars cars)
        {
            ScoreAndCarRequests = new List<GetScoreAndCarRequest>();
            foreach(var obj in entries.Leaderboards)
            {
                //ScoreAndCarRequests.Add(new GetScoreAndCarRequest(obj, cars));
            }
        }
    }
}