using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Domain
{
    public class LeaderboardService
    {

        private readonly Persistence.RedisDatastore redisDatastore;
        public LeaderboardService(RedisDatastore redisDatastore) {
            this.redisDatastore = redisDatastore;
        }
        
           public void SaveScoreAndCar(string trackName, string userName, double time, int carId, int skinId)
        {
            redisDatastore.SaveScoreAndCar(trackName, userName, time, carId, skinId);

        }


        public async Task<Leaderboard> GetScores(string trackName)
        {
            return await redisDatastore.GetScores(trackName);
        }


        public void SaveCar(string userName, int carId, int skinId)
        {
            redisDatastore.SaveCar(userName, carId, skinId);
        }

        public async Task<Cars> GetCar(string userName)
        {
            return await redisDatastore.GetCar(userName);
        }

        public async Task<LeaderboardList> BindLeaderboardInfo(string trackName)
        {
            // Getting Score

            var scoreResponse = await redisDatastore.GetScores(trackName);

            // Getting Car

            var currentUserId = new GetCurrentUserId(scoreResponse);

            var carResponse = await redisDatastore.GetCar(currentUserId.currentUserId);
            
            var bind = new LeaderboardList(scoreResponse, carResponse);

            return bind;
        }
    }
}