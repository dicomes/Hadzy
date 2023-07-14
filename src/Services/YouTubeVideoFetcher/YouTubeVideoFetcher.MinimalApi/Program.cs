using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using YouTubeVideoFetcher.MinimalApi;
using YouTubeVideoFetcher.MinimalApi.Endpoints;
using YouTubeVideoFetcher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions(); // Add this line

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    if (hostingContext.HostingEnvironment.IsDevelopment())
    {
        config.AddUserSecrets<Program>();
    }
});

var apiKey = builder.Configuration["YouTube:ApiKey"];
Console.WriteLine($"API Key used: {apiKey}");

builder.Services.AddSingleton<YouTubeService>(provider => new YouTubeService(new BaseClientService.Initializer
{
    ApiKey = apiKey
}));

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IVideoService, VideoService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureVideoEndpoints();

app.Run();
