using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Persistence
{
    public class RedisDatastore
    {

        // constructor com IOptions<RedisOptions> => endpoint, password, port

        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
        
        public void SaveScore(string trackName, string name, double time)
        {
            IDatabase db = redis.GetDatabase();

            db.SortedSetAdd(trackName, name, time);
        }


        public async Task<Leaderboard> GetScores(string trackName)
        {
            IDatabase db = redis.GetDatabase();

            var objs = await db.SortedSetRangeByRankWithScoresAsync(trackName);

            var response = new Leaderboard(objs);

            return response;
        } 
        
    }
}