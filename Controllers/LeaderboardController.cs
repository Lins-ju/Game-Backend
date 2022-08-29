using Backend.Domain;
using Backend.Models;
using Backend.Models.S3;
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


        [Route("saveleaderboards")]
        [HttpPost]

        public async void SaveLeaderboardRecords(SaveScoreRequest saveScoreRequest)
        {
            await leaderboardService.SaveLeaderboardDetails(saveScoreRequest.TrackId, saveScoreRequest.UserId, saveScoreRequest.Score,
            saveScoreRequest.CarId, saveScoreRequest.SkinId);
        }

        [Route("saveuser")]
        [HttpPost]

        public async void SaveUser()
        {
            
        }


        [Route("getleaderboards")]
        [HttpGet]

        public async Task<GetFullLeaderboard> GetLeaderboardRecords(GetLeaderboardRequest getLeaderboardRequest)
        {

            var bindedResult = await leaderboardService.GetLeaderboardRecords(getLeaderboardRequest.TrackId);

            return bindedResult;
        }

        [Route("getcars")]
        [HttpGet]

        public async Task<List<GetCarConfig>> GetCarsAvailable(ObjectKey objectKey)
        {
            var result = await leaderboardService.GetCarsAvailable();
            if (result.Count == 0)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        [Route("getcarcollection")]
        [HttpGet]

        public async Task<List<GetCarConfig>> GetCarConfigByPlayer(int playerId)
        {
            var response = await leaderboardService.GetCarsAvailableByPlayer(playerId);
            return response;
        }
    }
}