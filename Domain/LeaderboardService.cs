using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Domain
{
    public class LeaderboardService
    {

        private readonly Persistence.RedisDatastore redisDatastore;
        private readonly Persistence.DynamoDatastore dynamoDatastore;
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

        public async Task<GetFullLeaderboard> GetLeaderboardRecords(string trackId)
        {
            // Getting Score

            var scoreResponse = await redisDatastore.GetScores(trackId);
            var leaderboardDetailsResponse = await dynamoDatastore.LeadeboardDataListByTrackId(trackId);

            var leaderboardDetailResponse = new GetFullLeaderboard();

            var leaderboard = dynamoDatastore.DisplayFullLeaderboard(scoreResponse, leaderboardDetailsResponse);

            leaderboardDetailResponse.AddLeaderboardDetail(leaderboard);

            return leaderboardDetailResponse;
        }
    }
}