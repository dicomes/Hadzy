using YouTubeVideoFetcher.MinimalApi.Extensions;
using YouTubeVideoFetcher.MinimalApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

var seqConfig = builder.Configuration.GetSection("Seq").Get<SeqConfig>();
var youTubeConfig = builder.Configuration.GetSection("YouTube").Get<YouTubeConfig>();

// Builder extension methods
builder.UseSeqLogger(seqConfig);
builder.ConfigureUserSecretsInDevelopment();

// Service registration extension methods
builder.Services.AddYouTubeService(youTubeConfig);
builder.Services.AddCustomServices();

// Service registration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("API KEY: {ApiKey}", youTubeConfig.ApiKey);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.ConfigureVideoEndpoints();
app.Run();