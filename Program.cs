using Amazon.DynamoDBv2;
using Backend;
using Backend.Controllers;
using Backend.Domain;
using Backend.Models;
using Backend.Persistence;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);

app.Run();
