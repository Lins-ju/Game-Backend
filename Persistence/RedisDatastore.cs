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
        
        public void SaveScore(string trackName, string name, double time)
        {
            db.SortedSetAdd(trackName, name, time);
        }


        public async Task<Leaderboard> GetScores(string trackName)
        {
            var objs = await db.SortedSetRangeByRankWithScoresAsync(trackName);

            var response = new Leaderboard(objs);

            return response;
        }

        public async void SaveCar(string userName, string carName, int maxSpeed, int skinId)
        {
            var carSelected =  new Cars(carName, maxSpeed, skinId);

            var carSerialized = JsonSerializer.Serialize(carSelected, option);

            var post = await db.StringSetAsync(userName, carSerialized);
        }
        
    }
}