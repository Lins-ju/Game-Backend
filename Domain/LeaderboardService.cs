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
        
           public void SaveLeaderboardDetails(string trackId, string userId, double time, int carId, int skinId)
        {
            redisDatastore.SaveLeaderboardDetails(trackId, userId, time, carId, skinId);

        }


        public async Task<Leaderboard> GetScores(string trackId)
        {
            return await redisDatastore.GetScores(trackId);
        }

        public async Task<GetLeaderboardResponse> GetLeaderboardRecords(string trackId)
        {
            // Getting Score

            var scoreResponse = await redisDatastore.GetScores(trackId);

            var response2 = new GetLeaderboardResponse();
            foreach(var item in scoreResponse.Leaderboards)
            {
                var leaderboardDetails = await redisDatastore.GetLeaderboardDetails(item.UserId, trackId);
                response2.AddLeaderboardRecord(new LeaderboardRecord(item, leaderboardDetails));
            }
                
            return response2;
        }
    }
}