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
            // Getting Score

            var scoreResponse = await _redisDatastore.GetScores(trackId);

            var leaderboardDetailsResponse = await _dynamoDatastore.LeadeboardDataListByTrackId(trackId);

            var FullLeaderboard = new GetFullLeaderboard();

            LeaderboardEntry leaderboardEntry = new LeaderboardEntry();
            Leaderboard leaderboardReconstructed = new Leaderboard();

            if(scoreResponse.Leaderboards.Count == 0)
            {
                foreach(var item in leaderboardDetailsResponse)
                {
                    leaderboardEntry.UserId = item.UserId;
                    leaderboardEntry.Score = item.Properties.Score;
                    leaderboardReconstructed.Leaderboards.Add(leaderboardEntry);
                }
            }

            if(scoreResponse.Leaderboards.Count != 0)
            {
                foreach(var item in scoreResponse.Leaderboards)
                {
                    leaderboardEntry.UserId = item.UserId;
                    leaderboardEntry.Score = item.Score;
                    leaderboardReconstructed.Leaderboards.Add(leaderboardEntry);
                }
            }


            var leaderboard = _dynamoDatastore.DisplayFullLeaderboard(leaderboardReconstructed, leaderboardDetailsResponse);

            FullLeaderboard.AddLeaderboardDetail(leaderboard);

            return FullLeaderboard;
        }
    }
}