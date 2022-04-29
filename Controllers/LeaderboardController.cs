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

        public void SaveLeaderboardRecords(SaveScoreRequest saveScoreRequest)
        {
            leaderboardService.SaveLeaderboardDetails(saveScoreRequest.TrackId, saveScoreRequest.UserId, saveScoreRequest.Score, saveScoreRequest.CarId, saveScoreRequest.SkinId);
        }


        [Route("get")]
        [HttpGet]

        public async Task<GetLeaderboardResponse> GetLeaderboardRecords(GetLeaderboardRequest getLeaderboardRequest)
        {

            var bindedResult = await leaderboardService.GetLeaderboardRecords(getLeaderboardRequest.TrackId);

            return bindedResult;
        }
    }
}