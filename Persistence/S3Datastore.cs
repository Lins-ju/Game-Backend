using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Models;
using Backend.Models.S3;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

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
        public PutObjectRequest ObjectPostImage(IFormFile imgFile)
        {
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = "images/" + RandomGuid(),
                InputStream = imgFile.OpenReadStream()
            };
            return putObject;
        }

        public async Task<IFormFile> ObjectGetImageByKey(string imgKey)
        {
            var imgResponse = await _s3Buckets.GetObjectAsync(bucketParameter, imgKey);
            var filePath = TransformToImageAndSave(imgResponse.ResponseStream);
            var imageToIFormFile = ImageToIFormFile(filePath);
            return imageToIFormFile;
        }

        public int RandomNum()
        {
            Random rd = new Random();
            return rd.Next();
        }

        private string TransformToImageAndSave(Stream stream)
        {
            var streamToByte = ConvertStreamToByteArray(stream);
            MemoryStream ms = new MemoryStream(streamToByte);
            Image imgFromStream = Image.FromStream(ms);
            string fileName = Guid.NewGuid().ToString();
            string filePath = @$"C:\Users\JulioLins\Desktop\GamePA\Backend\Images\{fileName}.{imgFromStream.RawFormat}";
            imgFromStream.Save(filePath, ImageFormat.Png);

            return filePath;
        }

        public async Task<List<string>> ListAllObjects(string prefix)
        {
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketParameter,
                Prefix = prefix + "/",
                Delimiter = "/"
            };

            ListObjectsResponse listResponse;
            List<string> objectKeyList = new List<string>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsAsync(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    objectKeyList.Add(obj.Key);
                }

                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);

            return objectKeyList;
        }

        public async Task<List<string>> ListAllObjects()
        {
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketParameter
            };

            ListObjectsResponse listResponse;
            List<string> objectKeyList = new List<string>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsAsync(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    objectKeyList.Add(obj.Key);
                }

                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);

            return objectKeyList;
        }
        public async Task<bool> SaveUserProfileImg(string id, IFormFile imgFile)
        {
            var putImgObject = ObjectPostImage(imgFile);
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

        public async Task<bool> SaveCarConfig(string carName, int maxSpeed, CarType carType, IFormFile carImgUrl)
        {
            string skinId = RandomGuid();
            var skinResponse = await SaveSkinConfig(skinId, carImgUrl);

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

        //Development purposes
        public async Task<bool> SaveCarConfig(string carId, string skinId, string carName, int maxSpeed, CarType carType, IFormFile skinImg)
        {
            var skinResponse = await SaveSkinConfig(skinId, skinImg);

            CarConfig carConfig = new CarConfig(carId, carName, maxSpeed, carType, skinId);
            string carConfigSerialized = JsonSerializer.Serialize(carConfig, options);

            var putObjectCar = ObjectPostJson(carConfigSerialized, "cars");

            var response = await _s3Buckets.PutObjectAsync(putObjectCar);

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

        public async Task<bool> SaveSkinConfig(string skinImgUrl)
        {
            SkinConfig skinConfig = new SkinConfig(RandomGuid(), skinImgUrl);
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

        //Development purposes
        public async Task<bool> SaveSkinConfig(string id, IFormFile skinImg)
        {
            var postImg = ObjectPostImage(skinImg);
            SkinConfig skinConfig = new SkinConfig(id, postImg.Key);
            string trackConfigSerialized = JsonSerializer.Serialize(skinConfig, options);

            var putObjectJson = ObjectPostJson(trackConfigSerialized, "skin");

            var response = await _s3Buckets.PutObjectAsync(putObjectJson);
            var responseImg = await _s3Buckets.PutObjectAsync(postImg);


            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
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
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketParameter,
                Prefix = "cars/",
                Delimiter = "/"
            };

            ListObjectsResponse listResponse;
            List<CarConfig> carConfigList = new List<CarConfig>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsAsync(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    var response = await _s3Buckets.GetObjectAsync(bucketParameter, obj.Key);
                    using var reader = new StreamReader(response.ResponseStream);
                    var fileContent = await reader.ReadToEndAsync();
                    var fileDeserialized = JsonSerializer.Deserialize<CarConfig>(fileContent);
                    carConfigList.Add(fileDeserialized);
                }

                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);

            return carConfigList;
        }

        public async Task<RequestCarConfig> GetCarConfigListByCarId(string carId)
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
                    skin = skinConfigFromObject;
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