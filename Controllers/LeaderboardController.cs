using Backend.Domain;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/leaderboard")]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardService leaderboardService;

        public LeaderboardController(LeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService;
        }


        [Route("save")]
        [HttpPost]

        public void SaveScore(SaveScoreRequest saveScoreRequest)
        {
            leaderboardService.SaveScore(saveScoreRequest.TrackName, saveScoreRequest.UserId, saveScoreRequest.Score);
        }

        public void SaveCar()
        {
            
        }


        [Route("get")]
        [HttpGet]

        public async Task<List<LeaderboardEntry>> GetScores(GetScoreResponse getScoreResponse)
        {
            var scores = await leaderboardService.GetScores(getScoreResponse.TrackName);

            return scores.Leaderboards;
        }
    }
}