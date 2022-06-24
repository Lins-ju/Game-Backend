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

        public async void SaveLeaderboardRecords(SaveScoreRequest saveScoreRequest)
        {
            await leaderboardService.SaveLeaderboardDetails(saveScoreRequest.TrackId, saveScoreRequest.UserId, saveScoreRequest.Score);
        }


        [Route("get")]
        [HttpGet]

        public async Task<GetFullLeaderboard> GetLeaderboardRecords(GetLeaderboardRequest getLeaderboardRequest)
        {

            var bindedResult = await leaderboardService.GetLeaderboardRecords(getLeaderboardRequest.TrackId);

            return bindedResult;
        }
    }
}