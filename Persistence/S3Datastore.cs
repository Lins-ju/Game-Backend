using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Models;
using Backend.Models.S3;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using Amazon.Runtime;

namespace Backend.Persistence
{
    public class S3Datastore
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        public AmazonS3Client _s3Buckets;
        public string bucketParameter;
        public string RandomGuid() 
        { 
            return Guid.NewGuid().ToString();
        }
        public S3Datastore(AmazonS3Client s3Client, string bucketName)
        {
            bucketParameter = bucketName;
            _s3Buckets = s3Client;
        }

        public S3Datastore()
        {
            var creds = new BasicAWSCredentials("fakeMyKeyId", "fakeSecretAccessKey");
            var clientConfigS3 = new AmazonS3Config
            {
                ServiceURL = "http://localhost:4566",
                AuthenticationRegion = "us-east-1",
                ForcePathStyle = true
            };
            var amazonS3Client = new AmazonS3Client(creds, clientConfigS3);
            
            bucketParameter = "leaderboard-configs";
            _s3Buckets = amazonS3Client;
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

        public byte[] ConvertStreamToByteArray(Stream stream)
        {
            byte[] byteArray = new byte[16 * 1024];
            using (MemoryStream mStream = new MemoryStream())
            {
                int bit;
                while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
                {
                    mStream.Write(byteArray, 0, bit);
                }
                return mStream.ToArray();
            }
        }

        public string StreamToBase64(Stream stream)
        {
            byte[] bytes;
            using(var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
        public IFormFile ImageToIFormFile(string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                var formFile = new FormFile(stream, 0, stream.Length, "imageTest", Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };
                return formFile;
            }
        }

        public PutObjectRequest ObjectPostJson(string contentSerialized)
        {
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = RandomGuid(),
                ContentBody = contentSerialized,
                ContentType = "application/json"
            };
            return putObject;
        }

        public PutObjectRequest ObjectPostJson(string contentSerialized, string prefix)
        {
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = $"{prefix}/" + RandomGuid(),
                ContentBody = contentSerialized,
                ContentType = "application/json"
            };
            return putObject;
        }
        public PutObjectRequest ObjectPostImage(Stream imgStream)
        {
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = "images/" + RandomGuid(),
                InputStream = imgStream
            };
            return putObject;
        }

        public async Task<string> ObjectGetImageByKey(string imgKey)
        {
            var imgResponse = await _s3Buckets.GetObjectAsync(bucketParameter, imgKey);
            using var reader = new StreamReader(imgResponse.ResponseStream);
            var imgBase64String = await reader.ReadToEndAsync();
            return imgBase64String;
        }

        public int RandomNum()
        {
            Random rd = new Random();
            return rd.Next();
        }

        private string TransformToImageToBase64(Stream stream)
        {
            var streamToByte = StreamToBase64(stream);
            return streamToByte;
        }

        public async Task<List<string>> ListAllObjects(string prefix)
        {
            ListObjectsV2Request listRequest = new ListObjectsV2Request
            {
                BucketName = bucketParameter,
                Prefix = prefix + "/",
                Delimiter = "/"
            };

            ListObjectsV2Response listResponse;
            List<string> objectKeyList = new List<string>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsV2Async(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    objectKeyList.Add(obj.Key);
                }
                listRequest.ContinuationToken = listResponse.NextContinuationToken;
            } while (listResponse.IsTruncated);

            return objectKeyList;
        }

        public async Task<List<string>> ListAllObjects()
        {
            ListObjectsV2Request listRequest = new ListObjectsV2Request
            {
                BucketName = bucketParameter
            };

            ListObjectsV2Response listResponse = new ListObjectsV2Response();
            List<string> objectKeyList = new List<string>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsV2Async(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    objectKeyList.Add(obj.Key);
                }
                listRequest.ContinuationToken = listResponse.NextContinuationToken;
            } while (listResponse.IsTruncated);

            return objectKeyList;
        }

        public async Task<bool> SaveLeaderboardTrackId(string trackId)
        {
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = "trackids/" + trackId
            };

            var response = await _s3Buckets.PutObjectAsync(putObject);

            if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public async Task<bool> SaveUserProfileImg(string id, string imgBase64)
        {
            var putImgObject = ObjectPostJson(imgBase64);
            UserProfileImg playerConfig = new UserProfileImg(id, putImgObject.Key);
            string playerConfigSerialized = JsonSerializer.Serialize(playerConfig, options);

            var putObject = ObjectPostJson(playerConfigSerialized);
            var response = await _s3Buckets.PutObjectAsync(putObject);
            var imgResponse = await _s3Buckets.PutObjectAsync(putImgObject);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK && imgResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> SaveCarConfig(string carName, int maxSpeed, CarType carType, string carImgUrl)
        {
            string skinId = RandomGuid();
            var postSkinImg = ObjectPostJson(carImgUrl);
            var putObjectImg = await _s3Buckets.PutObjectAsync(postSkinImg);
            var skinResponse = await SaveSkinConfig(skinId, postSkinImg.Key);


            CarConfig carConfig = new CarConfig(RandomGuid(), carName, maxSpeed, carType, skinId);
            string carConfigSerialized = JsonSerializer.Serialize(carConfig, options);

            var putObject = ObjectPostJson(carConfigSerialized, "cars");

            var response = await _s3Buckets.PutObjectAsync(putObject);

            if (response.HttpStatusCode == HttpStatusCode.OK && skinResponse)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SaveTrackConfig(string trackName)
        {
            TrackConfig trackConfig = new TrackConfig(RandomGuid(), trackName);
            string trackConfigSerialized = JsonSerializer.Serialize(trackConfig, options);

            var putTrackObject = ObjectPostJson(trackConfigSerialized);

            var response = await _s3Buckets.PutObjectAsync(putTrackObject);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SaveSkinConfig(string id, string skinImgUrl)
        {
            SkinConfig skinConfig = new SkinConfig(id, skinImgUrl);
            string trackConfigSerialized = JsonSerializer.Serialize(skinConfig, options);

            var putObject = ObjectPostJson(trackConfigSerialized, "skin");

            var response = await _s3Buckets.PutObjectAsync(putObject);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<string>> GetTrackIds()
        {
            var listOfKeys = await ListAllObjects("trackids");
            return listOfKeys;
        }

        public async Task<UserProfileImg> GetUserProfileImg(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var playerConfigDeserialized = JsonSerializer.Deserialize<UserProfileImg>(fileContent, options);

            if (playerConfigDeserialized != null)
            {
                return playerConfigDeserialized;
            }
            else
            {
                return null;
            }
        }
        public async Task<GetTrackConfigResponse> GetTrackConfigResponse(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var trackConfigDeserialized = JsonSerializer.Deserialize<TrackConfig>(fileContent, options);

            GetTrackConfigResponse getTrackConfig = new GetTrackConfigResponse(trackConfigDeserialized.Id);

            if (trackConfigDeserialized != null)
            {
                return getTrackConfig;
            }
            else
            {
                return null;
            }
        }
        public async Task<RequestCarConfig> GetCarConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var carConfigDeserialized = JsonSerializer.Deserialize<CarConfig>(fileContent, options);
            var skinId = await GetSkinConfigBySkinId(carConfigDeserialized.SkinId);
            var carImg = await ObjectGetImageByKey(skinId.SkinImgUrl);

            RequestCarConfig getCarConfig = new RequestCarConfig(carConfigDeserialized, carImg);

            if (carConfigDeserialized != null)
            {
                return getCarConfig;
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
            if (skinConfigDeserialized != null)
            {
                return skinConfigDeserialized;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<CarConfig>> GetCarConfigAvailableList()
        {
            ListObjectsV2Request listRequest = new ListObjectsV2Request
            {
                BucketName = bucketParameter,
                Prefix = "cars/",
                Delimiter = "/"
            };

            ListObjectsV2Response listResponse;
            List<CarConfig> carConfigList = new List<CarConfig>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsV2Async(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    var response = await _s3Buckets.GetObjectAsync(bucketParameter, obj.Key);
                    using var reader = new StreamReader(response.ResponseStream);
                    var fileContent = await reader.ReadToEndAsync();
                    var fileDeserialized = JsonSerializer.Deserialize<CarConfig>(fileContent);
                    carConfigList.Add(fileDeserialized);
                }

                listRequest.ContinuationToken = listResponse.NextContinuationToken;
            } while (listResponse.IsTruncated);

            return carConfigList;
        }

        public async Task<RequestCarConfig> GetCarConfigByCarId(string carId)
        {
            RequestCarConfig requestCarConfig = new RequestCarConfig();
            var getCarsAvailable = await ListAllObjects("cars");
            foreach (var objectKey in getCarsAvailable)
            {
                var getCarConfigResponse = await GetCarConfig(objectKey);
                if (getCarConfigResponse.Id == carId)
                {
                    requestCarConfig.Id = carId;
                    requestCarConfig.CarName = getCarConfigResponse.CarName;
                    requestCarConfig.MaxSpeed = getCarConfigResponse.MaxSpeed;
                    requestCarConfig.CarImg = getCarConfigResponse.CarImg;
                    requestCarConfig.CarType = getCarConfigResponse.CarType;
                    requestCarConfig.SkinId = getCarConfigResponse.SkinId;
                }
            }

            return requestCarConfig;
        }

        public async Task<SkinConfig> GetSkinConfigBySkinId(string skinId)
        {
            SkinConfig skin = new SkinConfig();
            var getSkin = await ListAllObjects("skin");
            foreach (var objectKey in getSkin)
            {
                var skinConfigFromObject = await GetSkinConfig(objectKey);
                if (skinConfigFromObject.Id == skinId)
                {
                    skin.Id = skinConfigFromObject.Id;
                    skin.SkinImgUrl = skinConfigFromObject.SkinImgUrl;
                    skin.Type = skinConfigFromObject.Type;
                }
            }

            return skin;
        }

        public async Task<UserProfileImg> GetUserProfileImgById(string playerId)
        {
            UserProfileImg playerConfig = new UserProfileImg();
            var fileList = await ListAllObjects();
            foreach (var objectKey in fileList)
            {
                var objectFromS3 = await GetFileSerialized(objectKey);
                var json = JsonDocument.Parse(objectFromS3);
                var jsonElement = json.RootElement;
                var typeOf = jsonElement.GetProperty("type").GetString();
                var rawText = jsonElement.GetRawText();
                if (typeOf == "player")
                {
                    var playerDeserialized = JsonSerializer.Deserialize<UserProfileImg>(rawText, options);
                    if (playerDeserialized.Id == playerId)
                    {
                        playerConfig.Id = playerDeserialized.Id;
                        playerConfig.UserImgUrl = playerDeserialized.UserImgUrl;
                    }
                }
            }

            return playerConfig;
        }
    }
}