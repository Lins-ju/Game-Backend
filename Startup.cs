using Amazon.DynamoDBv2;

namespace Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        
        var awsConfig = Configuration.GetSection("Dynamo");
        var runLocalDynamoDb = awsConfig.GetValue<bool>("LocalMode");

        if (runLocalDynamoDb)
        {
            services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var clientConfig = new AmazonDynamoDBConfig
                {
                    ServiceURL = awsConfig.GetValue<string>("ServiceURL")
                };
                return new AmazonDynamoDBClient(clientConfig);
            });
        }
        else
        {
            services.AddAWSService<IAmazonDynamoDB>();
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        
    }
    }
}