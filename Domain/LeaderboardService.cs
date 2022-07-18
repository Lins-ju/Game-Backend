using Backend.Models;
using Backend.Persistence;

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

        public async Task<bool> SaveLeaderboardDetails(string trackId, string userId, double score)
        {
            var result = await _redisDatastore.SaveLeaderboard(trackId, userId, score);
            return result;
        }


        public async Task<Leaderboard> GetScores(string trackId)
        {
            return await _redisDatastore.GetScores(trackId);
        }

        public async Task<GetFullLeaderboard> GetLeaderboardRecords(string trackId)
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