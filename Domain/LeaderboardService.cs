using Backend.Models;
using Backend.Persistence;

namespace Backend.Domain
{
    public class LeaderboardService
    {

        private readonly Persistence.RedisDatastore _redisDatastore;
        private readonly Persistence.DynamoDatastore _dynamoDatastore;
        public LeaderboardService(RedisDatastore redisDatastore, DynamoDatastore dynamoDatastore)
        {
            _redisDatastore = redisDatastore;
            _dynamoDatastore = dynamoDatastore;

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
    }
}