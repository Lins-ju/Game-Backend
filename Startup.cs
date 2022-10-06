using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Backend.Controllers;
using Backend.Domain;
using Backend.Persistence;
using Microsoft.Extensions.Options;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // RedisDatastore redisDatastore = new RedisDatastore();
            // S3Datastore s3Datastore = new S3Datastore();
            // DynamoDatastore dynamoDatastore = new DynamoDatastore();

            // LeaderboardService leaderboardService = new LeaderboardService(redisDatastore, dynamoDatastore, s3Datastore);

            //services.AddMvc();
            services.AddRazorPages();
            services.AddControllers();
            services.AddCors();

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.Configure<RedisOptions>(Configuration.GetSection(RedisOptions.Section));
            services.Configure<DynamoOptions>(Configuration.GetSection(DynamoOptions.Section));

            var awsConfig = Configuration.GetSection(DynamoOptions.Section);
            var runLocalDynamoDb = awsConfig.GetValue<bool>("LocalMode");

            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = awsConfig.GetValue<string>("ServiceURL") };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }



            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:AccessKey"]);
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID", Configuration["AWS:SecretKey"]);
            Environment.SetEnvironmentVariable("AWS_REGION", Configuration["AWS:Region"]);

            services.AddAWSService<IAmazonDynamoDB>();
            services.AddAWSService<AmazonS3Client>();
            services.AddSingleton<ToolsController>();
            services.AddSingleton<GameViewController>();
            services.AddSingleton<LeaderboardService>();
            services.AddSingleton<S3Datastore>();
            services.AddSingleton<DynamoDatastore>();
            services.AddSingleton<RedisDatastore>();
        }

        public void Configure(WebApplication app, IWebHostEnvironment environment)
        {
            // if (app.Environment.IsDevelopment())
            // {
            //     app.UseSwagger();
            //     app.UseSwaggerUI();
            // }
            app.UseCors(
                options => options.WithOrigins("https://localhost:3000/")
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
            );

            app.UseStaticFiles();
            app.UseRouting();

            //app.UseMvc();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}