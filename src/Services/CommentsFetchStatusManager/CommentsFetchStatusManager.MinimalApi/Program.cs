using CommentsFetchStatus.MinimalApi.Configurations;
using CommentsFetchStatus.MinimalApi.Extensions;
using CommentsFetchStatus.MinimalApi.Mapper;
using CommentsFetchStatus.MinimalApi.Services;
using CommentsFetchStatus.MinimalApi.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var seqConfig = builder.Configuration.GetSection("Seq").Get<SeqConfig>();
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
var mongoDbConfig = builder.Configuration.GetSection("MongoDb").Get<MongoDbConfig>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq(seqConfig.Url)
    .WriteTo.Console()
    .CreateLogger();

builder.Services.Configure<MongoDbConfig>(
    builder.Configuration.GetSection("MongoDb"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbConfig>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddSingleton<IFetchStatusService, FetchStatusService>();

var app = builder.Build();
app.ConfigureFetchStatusEndpoints();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("CommentsFetchStatusManager service settings-------------------->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("RABBITMQ HOST: {RabbitMqHost}", rabbitMqConfig.Hostname);
logger.LogInformation("MONGODB CONNECTION: {MongoDbConnection}", mongoDbConfig.ConnectionString);
logger.LogInformation("<--------------------CommentsFetchStatusManager service settings");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
