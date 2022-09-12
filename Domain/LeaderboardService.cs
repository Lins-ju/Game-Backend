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


            return redisResult && dynamoResult;
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

        public async Task<List<RequestCarConfig>> GetCarsAvailable()
        {
            List<RequestCarConfig> carConfigList = new List<RequestCarConfig>();
            var carAvailableList = await _s3Datastore.GetCarConfigAvailableList();
            foreach (var car in carAvailableList)
            {
                var skinResponse = await _s3Datastore.GetSkinConfigBySkinId(car.SkinId);
                var imgResponse = await _s3Datastore.ObjectGetImageByKey(skinResponse.SkinImgUrl);
                RequestCarConfig carConfigElement = new RequestCarConfig(car, imgResponse);
                carConfigList.Add(carConfigElement);
            }
            return carConfigList;
        }

        public async Task<List<RequestCarConfig>> GetCarsAvailableByPlayer(string userName)
        {
            List<RequestCarConfig> carConfigList = new List<RequestCarConfig>();
            var dynamoResponse = await _dynamoDatastore.GetGameUserInfo(userName);
            var carCollectionList = dynamoResponse.CarCollectionList.carCollectionList;
            foreach (var item in carCollectionList)
            {
                var s3Response = await _s3Datastore.GetCarConfigListByCarId(item.CarId);
                carConfigList.Add(s3Response);
            }

            return carConfigList;
        }

        public async Task<bool> SaveUser(string userName, IFormFile userImg, CarCollectionList carCollectionList)
        {
            var playerId = new Random().Next();
            var dynamoResponse = await _dynamoDatastore.InsertUser(playerId, userName, carCollectionList);
            var s3Response = await _s3Datastore.SaveUserProfileImg(playerId, userImg);

            if (dynamoResponse && s3Response)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> SaveUser(int id, string userName, IFormFile userImg, CarCollectionList carCollectionList)
        {
            var dynamoResponse = await _dynamoDatastore.InsertUser(id, userName, carCollectionList);
            var s3Response = await _s3Datastore.SaveUserProfileImg(id, userImg);

            if (dynamoResponse && s3Response)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<GameUser> GetUserByUserName(string userName)
        {
            var dynamoResult = await _dynamoDatastore.GetGameUserInfo(userName);
            return dynamoResult;
        }

        public async Task<bool> SaveCar(string carName, int maxSpeed, CarType carType, IFormFile carImgUrl)
        {
            var response = await _s3Datastore.SaveCarConfig(carName, maxSpeed, carType, carImgUrl);
            return response;
        }
    }
}