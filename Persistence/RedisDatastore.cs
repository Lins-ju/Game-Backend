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

        public async void SaveCar(string userName, int carId, int skinId)
        {
            var carSelected =  new Cars(userName, carId, skinId);

            var carSerialized = JsonSerializer.Serialize(carSelected, option);

            var post = await db.StringSetAsync(userName, carSerialized);
        }

        public async Task<Cars> GetCar(string userName)
        {
            var getCarSerialized = await db.StringGetAsync(userName);

            var carDeserilized = JsonSerializer.Deserialize<Cars>(getCarSerialized);

            var carObj = new Cars(carDeserilized.UserName, carDeserilized.CarId, carDeserilized.SkinId);

            return carObj;
        }
        

        public async Task<GetScoreAndCarRequest> BindScoreAndCar(string trackName, string userName)
        {
            // Getting Score

            var scoreResponse = await GetScores(trackName);

            // Getting Car
            
            var carResponse = await GetCar(userName);
            

            var bind = new GetScoreAndCarRequest(scoreResponse, carResponse);

            return bind; 
        }
        
    }
}