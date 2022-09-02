using Backend.Domain;
using Backend.Models;
using Backend.Models.S3;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/tool")]
    public class GameAdminController
    {

        private readonly LeaderboardService leaderboardService;

        public GameAdminController(LeaderboardService leaderboardService)
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

        public async void SaveUser(SaveUserRequest saveUser)
        {
            await leaderboardService.SaveUser(saveUser.UserName, saveUser.UserImg, saveUser.CarCollectionList);
        }

        [Route("savecars")]
        [HttpPost]

        public async Task<bool> SaveCar(SaveCarRequest saveCar)
        {
            var result = await leaderboardService.SaveCar(saveCar.CarName, saveCar.MaxSpeed, saveCar.CarType, saveCar.CarSkinImg);
            return result;
        }
        
        [Route("getuserinfo")]
        [HttpGet]

        public async Task<List<RequestCarConfig>> GetCarConfigByPlayer(UserRequest user)
        {
            var response = await leaderboardService.GetCarsAvailableByPlayer(user.UserName);
            return response;
        }
    }
}