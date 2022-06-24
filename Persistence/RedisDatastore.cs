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
        private readonly Persistence.DynamoDatastore _dynamoDatastore;
        private readonly ConnectionMultiplexer redis;
        private readonly IDatabase db;
        private readonly JsonSerializerOptions option = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RedisDatastore(IOptions<RedisOptions> redisOptions)
        {
            redis = ConnectionMultiplexer.Connect($"{redisOptions.Value.Endpoint}:{redisOptions.Value.Port}");
            db = redis.GetDatabase();
        }
        public async Task<bool> SaveLeaderboard(string trackId, string userId, double score)
        {
            var postScore = await db.SortedSetAddAsync(trackId, userId, score);

            return postScore;
        }
        public async Task<Leaderboard> GetScores(string trackId)
        {
            var objs = await db.SortedSetRangeByRankWithScoresAsync(trackId);

            var response = new Leaderboard(objs);

            return response;
        }
    }
}