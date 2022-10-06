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

        public async Task<bool> SaveLeaderboardDetails(string trackId, string userId, double score, string carId, string skinId)
        {
            var redisResult = await _redisDatastore.SaveLeaderboard(trackId, userId, score);

            LeaderboardData leaderboardData = new LeaderboardData(trackId, userId, new Properties(carId, skinId, score));
            var dynamoResult = await _dynamoDatastore.Insert(leaderboardData);
            var saveTrackId = await _s3Datastore.SaveLeaderboardTrackId(trackId);

            if (redisResult && dynamoResult && saveTrackId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<Leaderboard> GetScores(string trackId)
        {
            return await _redisDatastore.GetScores(trackId);
        }

        public async Task<List<string>> GetTrackIdsForLeaderboard()
        {
            List<string> listOfRealKeys = new List<string>();
            var listOfKeys = await _s3Datastore.GetTrackIds();
            foreach (var key in listOfKeys)
            {
                var realKey = key.Replace("trackids/", "");
                listOfRealKeys.Add(realKey);
            }

            return listOfRealKeys;
        }

        public async Task<List<RequestLeaderboard>> GetLeaderboardRecords(string trackId)
        {
            List<RequestLeaderboard> leaderboardList = new List<RequestLeaderboard>();
            var scoreResponse = await _redisDatastore.GetScores(trackId);
            var leaderboardDetailsResponse = await _dynamoDatastore.LeadeboardDataListByTrackId(trackId);

            var leaderboard = _dynamoDatastore.DisplayFullLeaderboard(scoreResponse, leaderboardDetailsResponse);
            foreach(var item in leaderboard)
            {
                var s3result = await _s3Datastore.GetCarConfigByCarId(item.CarId);
                RequestLeaderboard leaderboardItem = new RequestLeaderboard(item.UserId, item.Score, s3result.CarName, s3result.CarImg);
                leaderboardList.Add(leaderboardItem);
            }

            return leaderboardList;
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
                var s3Response = await _s3Datastore.GetCarConfigByCarId(item.CarId);
                carConfigList.Add(s3Response);
            }

            return carConfigList;
        }

        public async Task<RequestCarConfig> GetCarConfigByCarId(string carId)
        {
            var s3Response = await _s3Datastore.GetCarConfigByCarId(carId);
            return s3Response;
        }

        public async Task<bool> SaveUser(string userName, string userImg, CarCollectionList carCollectionList)
        {
            string playerId = Guid.NewGuid().ToString();
            var dynamoResponse = await _dynamoDatastore.InsertUser(playerId, userName, carCollectionList);
            if (dynamoResponse)
            {
                var s3Response = await _s3Datastore.SaveUserProfileImg(playerId, userImg);
            }

            if (dynamoResponse)
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

        public async Task<bool> SaveCar(string carName, int maxSpeed, string carImgUrl, CarType carType)
        {
            var response = await _s3Datastore.SaveCarConfig(carName, maxSpeed, carType, carImgUrl);
            return response;
        }
    }
}