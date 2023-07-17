using System.Net;
using Google;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Diagnostics;
using YouTubeVideoFetcher.MinimalApi;
using YouTubeVideoFetcher.MinimalApi.Endpoints;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
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

app.UseExceptionHandler(app =>
{
    app.Run(async context =>
    {
        var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exception = errorFeature.Error;

        if (exception is VideoNotFoundException)
        {
            var errorResponse = VideoEndpoints.CreateErrorResponse<YouTubeVideoDto>(exception.Message);
            var result = Results.NotFound(errorResponse);
            await result.ExecuteAsync(context);
        }
        else if (exception is VideoBadRequestException)
        {
            var errorResponse = VideoEndpoints.CreateErrorResponse<YouTubeVideoDto>(exception.Message);
            var result = Results.BadRequest(errorResponse);
            await result.ExecuteAsync(context);
        }
        else if (exception is VideoAccessForbiddenException)
        {
            var result = Results.Forbid();
            await result.ExecuteAsync(context);
        }
        else if (exception is GoogleApiException)
        {
            var result = Results.StatusCode((int)HttpStatusCode.InternalServerError);
            await result.ExecuteAsync(context);
        }
        else
        {
            var result = Results.StatusCode((int)HttpStatusCode.InternalServerError);
            await result.ExecuteAsync(context);
        }
    });
});


app.ConfigureVideoEndpoints();

app.Run();

