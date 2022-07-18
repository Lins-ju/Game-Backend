using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Backend.Models;

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
            string PlayerConfigSerialized = JsonSerializer.Serialize(playerConfig, options);
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = PlayerConfigSerialized,
                ContentType = "application/json"
            };

            await _s3Buckets.PutObjectAsync(putObject);

            return $"Object Key is {objectKey}";
        }
    }
}