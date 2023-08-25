using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Serilog;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;
using MassTransit;
using YouTubeCommentsFetcher.Worker;
using YouTubeCommentsFetcher.Worker.Consumers;
using YouTubeCommentsFetcher.Worker.Mappers;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

var hostBuilder = Host.CreateDefaultBuilder(args);

SeqConfig seqConfig = null;
YouTubeConfig youTubeConfig = null;
RabbitMqConfig rabbitMqConfig = null;

hostBuilder
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var environment = hostingContext.HostingEnvironment;
        if (environment.IsDevelopment())
        {
            config.AddUserSecrets<Worker>();
        }
    })
    .ConfigureServices((hostingContext, services) =>
    {
        seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
        youTubeConfig = hostingContext.Configuration.GetSection("YouTube").Get<YouTubeConfig>();
        rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();

        // Logging Configuration
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig.Url)
            .WriteTo.Console()
            .CreateLogger();

        services.AddHostedService<Worker>();

        services.AddSingleton<YouTubeService>(sp => new YouTubeService(new BaseClientService.Initializer
        {
            ApiKey = youTubeConfig.ApiKey
        }));

        services.AddSingleton<RetryPolicyProvider>();
        services.AddTransient<FetcherErrorEventConsumer>();
        services.AddSingleton<IYouTubeFetcherService, YouTubeFetcherService>();
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddTransient<ICommentsThreadMapper, CommentsThreadMapper>();
        services.AddTransient<IYouTubeFetcherServiceExceptionHandler, YouTubeFetcherServiceExceptionHandler>();
        services.AddTransient<FetchStartedEventConsumer>();
        services.AddTransient<IEventPublisher, EventPublisher>();
        services.AddTransient<ICommentsOverlapHandler, CommentsOverlapHandler>();
        services.AddTransient<ICommentThreadIterator, CommentThreadIterator>();
        services.AddTransient<IIntegrationEventsManager, IntegrationEventsManager>();

        // MassTransit Configuration
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<FetchStartedEventConsumer>();
            configurator.AddConsumer<FetcherErrorEventConsumer>();

            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                {
                    configure.Username(rabbitMqConfig.User);
                    configure.Password(rabbitMqConfig.Password);
                });

                cfg.ReceiveEndpoint("videoId-queue", e =>
                {
                    e.Consumer<FetchStartedEventConsumer>(context);
                });

                cfg.ReceiveEndpoint("error-message-queue", e =>
                {
                    e.Consumer<FetcherErrorEventConsumer>(context);
                });
            });
        });
    })
    .UseSerilog();

var host = hostBuilder.Build();

// Logging Service Settings
var logger = host.Services.GetRequiredService<ILogger<Worker>>();
logger.LogInformation("YouTubeCommentsFetcher service settings-------------------->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("API KEY: {ApiKey}", youTubeConfig.ApiKey);
logger.LogInformation("RABBITMQ HOST: {RabbitMqHost}", rabbitMqConfig.Hostname);
logger.LogInformation("<--------------------YouTubeCommentsFetcher service settings");

await host.RunAsync();
