using Backend.Domain;
using Backend.Models;
using Backend.Models.S3;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/gameview")]
    public class LeaderboardController : ControllerBase
    {
        private readonly LeaderboardService leaderboardService;

        public LeaderboardController(LeaderboardService leaderboardService)
        {
            this.leaderboardService = leaderboardService;
        }

        [Route("getleaderboards")]
        [HttpGet]

        public async Task<GetFullLeaderboard> GetLeaderboardRecords(GetLeaderboardRequest getLeaderboardRequest)
        {

            var bindedResult = await leaderboardService.GetLeaderboardRecords(getLeaderboardRequest.TrackId);

            return bindedResult;
        }

        [Route("getcarsAvailable")]
        [HttpGet]

        public async Task<List<RequestCarConfig>> GetCarsAvailable()
        {
            var result = await leaderboardService.GetCarsAvailable();
            if (result.Count == 0)
            {
                return new List<RequestCarConfig>();
            }
            else
            {
                return result;
            }
        }
    }
}