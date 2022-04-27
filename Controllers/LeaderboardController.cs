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
            leaderboardService.SaveCar(saveScoreRequest.UserId, saveScoreRequest.CarId, saveScoreRequest.SkinId); // Player Data
        }


        [Route("get")]
        [HttpGet]

        public async Task<GetScoreAndCarRequest> GetLeaderboard(TrackAndUserKey trackAndUserKey)
        {

            var bindedResults = await leaderboardService.BindScoreAndCar(trackAndUserKey.TrackName, trackAndUserKey.UserName);

            return bindedResults;
        }
    }
}