using System.Text.Json;
using Backend.Domain;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Backend.Persistence
{
    public class RedisDatastore
    {

        // constructor com IOptions<RedisOptions> => endpoint, password, port

        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;
        private readonly JsonSerializerOptions option = new ()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RedisDatastore(IOptions<RedisOptions> redisOptions)
        {
            redis = ConnectionMultiplexer.Connect($"{redisOptions.Value.Endpoint}:{redisOptions.Value.Port}");
            db = redis.GetDatabase();
        }
        
        public async void SaveLeaderboardDetails(string trackId, string userId, double time, int carId, int skinId)
        {

            var postScore = await db.SortedSetAddAsync(trackId, userId, time);

            var detailsSelected =  new LeaderboardDetails(userId, carId, skinId);

            var detailsSerialized = JsonSerializer.Serialize(detailsSelected, option);

            var post = await db.StringSetAsync(GetLeaderboardDetailsKey(userId, trackId), detailsSerialized);
        }


        public async Task<Leaderboard> GetScores(string trackId)
        {
            var objs = await db.SortedSetRangeByRankWithScoresAsync(trackId);

            var response = new Leaderboard(objs);

            return response;
        }

        public async Task<LeaderboardDetails> GetLeaderboardDetails(string userId, string trackId)
        {
            var getdetailsSerialized = await db.StringGetAsync(GetLeaderboardDetailsKey(userId, trackId));

            var detailsDeserilized = JsonSerializer.Deserialize<LeaderboardDetails>(getdetailsSerialized);

            var detailsObj = new LeaderboardDetails(detailsDeserilized.UserId, detailsDeserilized.CarId, detailsDeserilized.SkinId);

            return detailsObj;
        }

        public string GetLeaderboardDetailsKey(string userId, string trackId)
        {
            return userId + ":" + trackId;
        }
    }
}