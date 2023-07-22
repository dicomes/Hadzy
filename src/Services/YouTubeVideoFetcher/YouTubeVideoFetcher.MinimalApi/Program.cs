using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Diagnostics;
using YouTubeVideoFetcher.MinimalApi;
using YouTubeVideoFetcher.MinimalApi.Endpoints;
using Serilog;
using YouTubeVideoFetcher.MinimalApi.Services;

var builder = WebApplication.CreateBuilder(args);

var seqUrl = builder.Configuration["Seq:Url"];
Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq(seqUrl)
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Configure user secrets
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    if (hostingContext.HostingEnvironment.IsDevelopment())
    {
        config.AddUserSecrets<Program>();
    }
});

var apiKey = builder.Configuration["YouTube:ApiKey"];
builder.Services.AddSingleton<YouTubeService>(provider => new YouTubeService(new BaseClientService.Initializer
{
    ApiKey = apiKey
}));

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IFetcherService, FetcherService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IVideoHandlerService, VideoHandlerService>();
builder.Services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("SEQ URL: {seqUrl}", seqUrl);
logger.LogInformation("API KEY: {apiKey}", apiKey);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(app =>
{
    app.Run(async context =>
    {
        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = errorFeature.Error;

        var handlerService = context.RequestServices.GetRequiredService<IExceptionHandlerService>();
        var result = await handlerService.HandleException(exception);
        await result.ExecuteAsync(context);
    });
});

app.ConfigureVideoEndpoints();

app.Run();