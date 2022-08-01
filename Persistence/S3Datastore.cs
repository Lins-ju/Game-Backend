using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Models;
using Backend.Models.S3;

namespace Backend.Persistence
{
    public class S3Datastore
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        public AmazonS3Client _s3Buckets;
        public string bucketParameter;
        public S3Datastore(AmazonS3Client s3Client, string bucketName)
        {
            bucketParameter = bucketName;
            _s3Buckets = s3Client;
        }
        public async Task<string> GetFileSerialized(string objectKey)
        {
           var objectRequest = new GetObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey
            };
            var getResponse = await _s3Buckets.GetObjectAsync(objectRequest);
            using var reader = new StreamReader(getResponse.ResponseStream);
            var fileContent = await reader.ReadToEndAsync();
            return fileContent;
        }
        public async Task<string> PostPlayerConfig(int carId, int skinId)
        {
            string objectKey = Guid.NewGuid().ToString();
            PlayerConfig playerConfig = new PlayerConfig(carId, skinId);
            string playerConfigSerialized = JsonSerializer.Serialize(playerConfig, options);
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = playerConfigSerialized,
                ContentType = "application/json"
            };

            var response = await _s3Buckets.PutObjectAsync(putObject);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"{objectKey}";
            }
            else
            {
                return "Object not posted";
            }

        }

        public async Task<string> PostCarConfig(int carId, string carName, int maxSpeed, CarType carType)
        {
            string objectKey = Guid.NewGuid().ToString();
            CarConfig carConfig = new CarConfig(carId, carName, maxSpeed, carType);
            string carConfigSerialized = JsonSerializer.Serialize(carConfig, options);
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = carConfigSerialized,
                ContentType = "application/json"
            };
            var response = await _s3Buckets.PutObjectAsync(putObject);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"{objectKey}";
            }
            else
            {
                return "Object not posted";
            }
        }

        public async Task<string> PostTrackConfig(string trackId, IFormFile imgFile)
        {
            string objectKey = Guid.NewGuid().ToString();
            string imgKey = Guid.NewGuid().ToString();
            TrackConfig trackConfig = new TrackConfig(trackId, imgKey);
            string trackConfigSerialized = JsonSerializer.Serialize(trackConfig, options);
            var putTrackObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = trackConfigSerialized,
                ContentType = "application/json"
            };
            var putTrackImg = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = imgKey,
                InputStream = imgFile.OpenReadStream(),
                ContentType = imgFile.ContentType
            };
            var response = await _s3Buckets.PutObjectAsync(putTrackObject);
            var imgResponse = await _s3Buckets.PutObjectAsync(putTrackImg);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK && imgResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"{objectKey} and image key is {imgKey}";
            }
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK && imgResponse.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return "Object and image not posted";
            }
            else
            {
                return "Object or image not posted";
            }
        }

        public async Task<string> PostSkinConfig(string skinColor, int carSkinType)
        {
            string objectKey = Guid.NewGuid().ToString();
            SkinConfig skinConfig = new SkinConfig(skinColor, (CarPaintType)carSkinType);
            string trackConfigSerialized = JsonSerializer.Serialize(skinConfig, options);
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = trackConfigSerialized,
                ContentType = "application/json"
            };
            var response = await _s3Buckets.PutObjectAsync(putObject);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"{objectKey}";
            }
            else
            {
                return "Object not posted";
            }
        }

        public async Task<PlayerConfig> GetPlayerConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var playerConfigDeserialized = JsonSerializer.Deserialize<PlayerConfig>(fileContent, options);
            if(playerConfigDeserialized != null)
            {
                return playerConfigDeserialized;
            }
            else
            {
                return null;
            }
        }
        public async Task<TrackConfig> GetTrackConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var trackConfigDeserialized = JsonSerializer.Deserialize<TrackConfig>(fileContent, options);
            using var imgContent = await _s3Buckets.GetObjectAsync(bucketParameter, trackConfigDeserialized.TrackImgId);
            {
                using (var responseStream = imgContent.ResponseStream)
                {
                    
                }
            }
            if(trackConfigDeserialized != null)
            {
                return trackConfigDeserialized;
            }
            else
            {
                return null;
            }
        }
        public async Task<CarConfig> GetCarConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var carConfigDeserialized = JsonSerializer.Deserialize<CarConfig>(fileContent, options);
            if(carConfigDeserialized != null)
            {
                return carConfigDeserialized;
            }
            else
            {
                return null;
            }
        }

        public async Task<SkinConfig> GetSkinConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var skinConfigDeserialized = JsonSerializer.Deserialize<SkinConfig>(fileContent, options);
            if(skinConfigDeserialized != null)
            {
                return skinConfigDeserialized;
            }
            else
            {
                return null;
            }
        }
    }
}