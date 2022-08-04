using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Domain
{
    public class LeaderboardService
    {

        private readonly Persistence.RedisDatastore _redisDatastore;
        private readonly Persistence.DynamoDatastore _dynamoDatastore;
        private readonly Persistence.S3Datastore _s3Datastore;
        public LeaderboardService(RedisDatastore redisDatastore, DynamoDatastore dynamoDatastore, S3Datastore s3Datastore)
        {
            _redisDatastore = redisDatastore;
            _dynamoDatastore = dynamoDatastore;
            _s3Datastore = s3Datastore;
        }
        
           public void SaveLeaderboardDetails(string trackId, string userId, double time, int carId, int skinId)
        {
            _redisDatastore.SaveLeaderboardDetails(trackId, userId, time, carId, skinId);

        }


        public async Task<Leaderboard> GetScores(string trackId)
        {
            return await _redisDatastore.GetScores(trackId);
        }

        public async Task<GetLeaderboardResponse> GetLeaderboardRecords(string trackId)
        {
            var scoreResponse = await _redisDatastore.GetScores(trackId);

            var leaderboardDetailsResponse = await _dynamoDatastore.LeadeboardDataListByTrackId(trackId);

            var FullLeaderboard = new GetFullLeaderboard();

            var leaderboard = _dynamoDatastore.DisplayFullLeaderboard(scoreResponse, leaderboardDetailsResponse);

            FullLeaderboard.AddLeaderboardDetail(leaderboard);

            return FullLeaderboard;
        }

        public async Task<string> PostPlayerConfig(int carId, int skinId)
        {
            var response = await _s3Datastore.PostPlayerConfig(carId, skinId);
            return response;
        }
    }
}