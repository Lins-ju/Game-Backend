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

        public void SaveLeaderboardInfo(SaveScoreRequest saveScoreRequest)
        {
            leaderboardService.SaveScoreAndCar(saveScoreRequest.TrackName, saveScoreRequest.UserId, saveScoreRequest.Score, saveScoreRequest.CarId, saveScoreRequest.SkinId);
        }


        [Route("get")]
        [HttpGet]

        public async Task<List<GetScoreAndCarRequest>> GetLeaderboardInfo(TrackKey trackKey)
        {

            var bindedResults = await leaderboardService.BindLeaderboardInfo(trackKey.TrackName);

            return bindedResults;
        }
    }
}