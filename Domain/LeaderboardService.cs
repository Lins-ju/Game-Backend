using Backend.Models;
using Backend.Persistence;

namespace Backend.Domain
{
    public class LeaderboardService
    {

        private readonly Persistence.RedisDatastore redisDatastore;
        public LeaderboardService(RedisDatastore redisDatastore) {
            this.redisDatastore = redisDatastore;
        }
        
           public void SaveScore(string trackName, string name, double time)
        {
            redisDatastore.SaveScore(trackName, name, time);

        }


        public async Task<Leaderboard> GetScores(string trackName)
        {
            return await redisDatastore.GetScores(trackName);
        }


        public async void SaveCar(string userName, int carId, int skinId)
        {
            redisDatastore.SaveCar(userName, carId, skinId);
        }

        public async Task<Cars> GetCar(string userName)
        {
            return await redisDatastore.GetCar(userName);
        }

        public async Task<GetScoreAndCarRequest> BindScoreAndCar(string trackName, string userName)
        {
            return await redisDatastore.BindScoreAndCar(trackName, userName);
        }
    }
}