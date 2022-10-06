using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Backend.Domain;
using Backend.Models;
using Backend.Models.S3;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/gameview")]
    [ApiController]
    public class GameViewController : ControllerBase
    {

        private readonly LeaderboardService _leaderboardService;

        public GameViewController(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [Route("getTrackIds")]
        [HttpGet]

        public async Task<List<string>> GetTrackIds()
        {
            var result = await _leaderboardService.GetTrackIdsForLeaderboard();
            return result;
        }

        [Route("getLeaderboards")]
        [HttpPost]

        public async Task<GetFullLeaderboard> GetLeaderboardRecords(GetLeaderboardRequest getLeaderboardRequest)
        {

            var bindedResult = await _leaderboardService.GetLeaderboardRecords(getLeaderboardRequest.TrackId);
            return bindedResult;
        }

        [Route("getCarByCarId")]
        [HttpPost]

        public async Task<RequestCarConfig> GetCarByCarConfig(string carId)
        {
            var result = await _leaderboardService.GetCarConfigByCarId(carId);
            return result;
        }

        [Route("getCarsAvailable")]
        [HttpGet]

        public async Task<List<RequestCarConfig>> GetCarsAvailable()
        {
            var result = await _leaderboardService.GetCarsAvailable();
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