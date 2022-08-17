using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Models;
using Backend.Models.S3;
using System.Drawing;
using System.Drawing.Imaging;

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
        public async Task<string> PostPlayerConfig(int carId, int skinId)
        {

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

            string imgKey = Guid.NewGuid().ToString();
            TrackConfig trackConfig = new TrackConfig(trackId, imgKey);
            string trackConfigSerialized = JsonSerializer.Serialize(trackConfig, options);
            await using var memoryStream = new MemoryStream();
            await imgFile.CopyToAsync(memoryStream);
            string base64String = Convert.ToBase64String(memoryStream.ToArray());
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
                ContentBody = base64String
            };
            var response = await _s3Buckets.PutObjectAsync(putTrackObject);
            var imgResponse = await _s3Buckets.PutObjectAsync(putTrackImg);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK && imgResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"{objectKey}";
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
            if (playerConfigDeserialized != null)
            {
                return playerConfigDeserialized;
            }
            else
            {
                return null;
            }
        }
        public async Task<GetTrackConfig> GetTrackConfig(string objectKey)
        {
            var fileContent = await GetFileSerialized(objectKey);
            var trackConfigDeserialized = JsonSerializer.Deserialize<TrackConfig>(fileContent, options);
            var imgContent = await _s3Buckets.GetObjectAsync(bucketParameter, trackConfigDeserialized.TrackImgUrl);
            var imgBytes = ConvertStreamToByteArray(imgContent.ResponseStream);
            string fileName = Guid.NewGuid().ToString();
            string filePath = @$"C:\Users\JulioLins\Desktop\GamePA\Backend\Images\{fileName}";
            await File.WriteAllBytesAsync(filePath, imgBytes);
            using MemoryStream ms = new MemoryStream(imgBytes, 0, imgBytes.Length);
            ms.Position = 0; // this is important
            var returnImage = Image.FromStream(ms,true);
            GetTrackConfig getTrackConfig = new GetTrackConfig();
            getTrackConfig.TrackId = trackConfigDeserialized.TrackId;
            using (var stream = File.OpenRead(filePath))
            {
                var formFile = new FormFile(stream, 0, stream.Length, "imageTest", Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };
                getTrackConfig.ImgFile = formFile;
            }

            if (trackConfigDeserialized != null)
            {
                return getTrackConfig;
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
            if (carConfigDeserialized != null)
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
            if (skinConfigDeserialized != null)
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