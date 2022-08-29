using Backend.Models;
using Backend.Models.S3;
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

        public async Task<bool> SaveLeaderboardDetails(string trackId, string userId, double score, int carId, int skinId)
        {
            var redisResult = await _redisDatastore.SaveLeaderboard(trackId, userId, score);

            LeaderboardData leaderboardData = new LeaderboardData(trackId, userId, new Properties(carId, skinId, score));
            var dynamoResult = await _dynamoDatastore.Insert(leaderboardData);


            return redisResult && dynamoResult ? true : false;
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

        public async Task<List<GetCarConfig>> GetCarsAvailable()
        {
            List<GetCarConfig> carConfigList = new List<GetCarConfig>();
            var carAvailableList = await _s3Datastore.GetCarConfigAvailableList();
            foreach(var car in carAvailableList)
            {
                var imgResponse = await _s3Datastore.ObjectGetImageByKey(car.CarImgUrl);
                GetCarConfig carConfigElement = new GetCarConfig(car, imgResponse);
                carConfigList.Add(carConfigElement);
            }
            return carConfigList;
        }

        public async Task<List<GetCarConfig>> GetCarsAvailableByPlayer(int playerId)
        {
            List<GetCarConfig> carConfigList = new List<GetCarConfig>();
            var dynamoResponse = await _dynamoDatastore.GetGameUserInfo(playerId);
            var carCollectionList = dynamoResponse.CarCollectionList.carCollectionList;
            foreach(var item in carCollectionList)
            {
                var s3Response = await _s3Datastore.GetCarConfigListByCarId(dynamoResponse.CarCollectionList);
                carConfigList = s3Response;
            }

            return carConfigList;
        }

        public async Task<bool> SaveUser(string userName, CarCollectionList carCollectionList, IFormFile userImg)
        {
            var playerId = new Random().Next();
            var dynamoResponse = await _dynamoDatastore.InsertUser(playerId, userName, carCollectionList);
            var s3Response = await _s3Datastore.PostPlayerConfig(playerId, userImg);
            var s3answer = s3Response != "Object not posted" ? true : false;

            if(dynamoResponse && s3answer)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}