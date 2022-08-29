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
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        public AmazonS3Client _s3Buckets;
        public string bucketParameter;

        public static string objectKey = Guid.NewGuid().ToString();
        public static string randomGuid = Guid.NewGuid().ToString();
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
                Key = objectKey,
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
                Key = $"{prefix}/{objectKey}",
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
                Key = objectKey,
                InputStream = imgFile.OpenReadStream()
            };
            return putObject;
        }

        public async Task<IFormFile> ObjectGetImageByKey(string imgKey)
        {
            var imgResponse = await _s3Buckets.GetObjectAsync(bucketParameter, imgKey);
            var filePath = TransformToImageAndSave(imgResponse);
            var imageToIFormFile = ImageToIFormFile(filePath);
            return imageToIFormFile;
        }

        public int RandomNum()
        {
            Random rd = new Random();
            return rd.Next();
        }

        private string TransformToImageAndSave(GetObjectResponse getObjectResponse)
        {
            Image imgFromStream = Image.FromStream(getObjectResponse.ResponseStream);
            string fileName = Guid.NewGuid().ToString();
            string filePath = @$"C:\Users\JulioLins\Desktop\GamePA\Backend\Images\{fileName}.{imgFromStream.RawFormat}";
            imgFromStream.Save(filePath, ImageFormat.Png);

            return filePath;
        }

        private async Task<List<string>> ListCars()
        {
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = bucketParameter,
                Prefix = "cars/",
                Delimiter = "/"
            };

            ListObjectsResponse listResponse;
            List<string> carObjectKeyList = new List<string>();

            do
            {
                listResponse = await _s3Buckets.ListObjectsAsync(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {
                    carObjectKeyList.Add(obj.Key);
                }

                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);

            return carObjectKeyList;
        }
        public async Task<string> PostPlayerConfig(int id, IFormFile imgFile)
        {
            var putImgObject = ObjectPostImage(imgFile);
            PlayerConfig playerConfig = new PlayerConfig(id, putImgObject.Key);
            string playerConfigSerialized = JsonSerializer.Serialize(playerConfig, options);

            var putObject = ObjectPostJson(playerConfigSerialized);
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

        public async Task<string> PostCarConfig(int carId, string carName, int maxSpeed, CarType carType, IFormFile carImgUrl)
        {
            var putImgObject = ObjectPostImage(carImgUrl);

            CarConfig carConfig = new CarConfig(carId, carName, maxSpeed, carType, putImgObject.Key);
            string carConfigSerialized = JsonSerializer.Serialize(carConfig, options);

            var putObject = ObjectPostJson(carConfigSerialized, "cars");

            var response = await _s3Buckets.PutObjectAsync(putObject);
            var imgResponse = await _s3Buckets.PutObjectAsync(putImgObject);

            if (response.HttpStatusCode == HttpStatusCode.OK && imgResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                return $"{objectKey}";
            }
            if (response.HttpStatusCode != HttpStatusCode.OK && imgResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                return "Object and image not posted";
            }
            else
            {
                return "Object or image not posted";
            }
        }

        public async Task<string> PostTrackConfig(string trackName)
        {
            TrackConfig trackConfig = new TrackConfig(RandomNum(), trackName);
            string trackConfigSerialized = JsonSerializer.Serialize(trackConfig, options);

            var putTrackObject = ObjectPostJson(trackConfigSerialized);

            var response = await _s3Buckets.PutObjectAsync(putTrackObject);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return $"{objectKey}";
            }
            else
            {
                return "Object not posted";
            }
        }

        public async Task<string> PostSkinConfig(int carSkinType)
        {
            SkinConfig skinConfig = new SkinConfig(RandomNum(), (CarPaintType)carSkinType);
            string trackConfigSerialized = JsonSerializer.Serialize(skinConfig, options);

            var putObject = ObjectPostJson(trackConfigSerialized);

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
        public async Task<GetCarConfig> GetCarConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var carConfigDeserialized = JsonSerializer.Deserialize<CarConfig>(fileContent, options);
            var carImg = await _s3Buckets.GetObjectAsync(bucketParameter, carConfigDeserialized.CarImgUrl);

            string filePath = TransformToImageAndSave(carImg);
            var carImgFormFile = ImageToIFormFile(filePath);

            GetCarConfig getCarConfig = new GetCarConfig(carConfigDeserialized, carImgFormFile);

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

        public async Task<List<GetCarConfig>> GetCarConfigListByCarId(CarCollectionList carCollectionList)
        {
            List<GetCarConfig> carConfigList = new List<GetCarConfig>();
            var getCarsAvailable = await ListCars();
            foreach(var objectKey in getCarsAvailable)
            {
                foreach(var carCollectionItem in carCollectionList.carCollectionList)
                {
                    var getCarConfigResponse = await GetCarConfig(objectKey);
                    if(getCarConfigResponse.Id == carCollectionItem.CarId)
                    {
                        carConfigList.Add(getCarConfigResponse);
                    }
                }
            }

            return carConfigList;
        }
    }
}