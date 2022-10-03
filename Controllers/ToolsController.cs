using System.Drawing;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Backend.Domain;
using Backend.Models;
using Backend.Models.S3;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Backend.Controllers
{
    [Route("api/tools")]
    [ApiController]
    public class ToolsController : ControllerBase
    {

        private readonly LeaderboardService _leaderboardService;

        public ToolsController(LeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [Route("saveLeaderboards")]
        [HttpPost]
        public async void SaveLeaderboardRecords(SaveScoreRequest saveScoreRequest)
        {
            await _leaderboardService.SaveLeaderboardDetails(saveScoreRequest.TrackId, saveScoreRequest.UserId, saveScoreRequest.Score,
            saveScoreRequest.CarId, saveScoreRequest.SkinId);
        }

        [Route("saveUser")]
        [HttpPost]
        public async Task<bool> SaveUser(SaveUserRequest saveUser)
        {
            var collectionList = new CarCollectionList(saveUser.CarCollectionList);
            var result = await _leaderboardService.SaveUser(saveUser.UserName, saveUser.UserProfileImg, collectionList);
            return result;
        }

        [Route("saveCars")]
        [HttpPost]
        public async Task<bool> SaveCar(SaveCarRequest saveCar)
        {
            var intToCarType = (CarType)saveCar.CarType;
            var result = await _leaderboardService.SaveCar(saveCar.CarName, saveCar.MaxSpeed, saveCar.CarSkinImg, intToCarType);
            return result;
            
        }

        [Route("getCarCollection")]
        [HttpGet]

        public async Task<List<RequestCarConfig>> GetCarConfigByPlayer(UserRequest user)
        {
            var response = await _leaderboardService.GetCarsAvailableByPlayer(user.UserName);
            return response;
        }
    }
}