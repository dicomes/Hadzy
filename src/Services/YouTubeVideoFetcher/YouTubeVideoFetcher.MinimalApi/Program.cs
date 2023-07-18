using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Diagnostics;
using YouTubeVideoFetcher.MinimalApi;
using YouTubeVideoFetcher.MinimalApi.Endpoints;
using YouTubeVideoFetcher.Services;

var builder = WebApplication.CreateBuilder(args);
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
Console.WriteLine($"API Key used: {apiKey}");

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOptions();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IFetcherService, FetcherService>();
builder.Services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();

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

        var handlerService = context.RequestServices.GetRequiredService<IExceptionHandlerService>();
        var result = await handlerService.HandleException(exception);
        await result.ExecuteAsync(context);
    });
});

app.ConfigureVideoEndpoints();

app.Run();