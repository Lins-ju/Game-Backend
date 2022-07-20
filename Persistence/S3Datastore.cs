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

        public Guid guid = new Guid();

        public async Task<string> PostPlayerConfig(int carId, int skinId)
        {
            string objectKey = guid.ToString();
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
            if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return $"Object Key is {objectKey}";
            }
            else
            {
                return "Object not posted";
            }

        }

        public async Task<string> PostCarConfig(int carId, string carName, int maxSpeed, CarType carType)
        {
            string objectKey = guid.ToString();
            CarConfig carConfig = new CarConfig(carId, carName, maxSpeed, carType);
            string carConfigSerialized = JsonSerializer.Serialize(carConfig, options);
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = carConfigSerialized,
                ContentType = "application/json"
            };

            await _s3Buckets.PutObjectAsync(putObject);

            return $"Object Key is {objectKey}";
        }
    }
}