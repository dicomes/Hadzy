using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Serilog;
using YouTubeCommentsFetcher.Worker.Configurations;
using YouTubeCommentsFetcher.Worker.Services;
using MassTransit;
using YouTubeCommentsFetcher.Worker;
using YouTubeCommentsFetcher.Worker.Consumers;
using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.Mappers;

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
        services.AddSingleton<IYouTubeFetcher, YouTubeFetcher>();
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddTransient<ICommentsThreadMapper, CommentsThreadMapper>();
        services.AddTransient<IYouTubeFetcherServiceExceptionHandler, YouTubeFetcherServiceExceptionHandler>();
        services.AddTransient<FetchStartedEventConsumer>();
        services.AddTransient<IEventPublisher, EventPublisher>();
        services.AddTransient<ICommentsOverlapHandler, CommentsOverlapHandler>();
        services.AddTransient<ICommentFetchIterator, CommentFetchIterator>();
        services.AddTransient<IEventsManager, EventsManager>();

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

                cfg.ReceiveEndpoint("comments-fetcher-fetch-started-consumer-endpoint", e =>
                {
                    e.Consumer<FetchStartedEventConsumer>(context);
                });

                cfg.ReceiveEndpoint("comments-fetcher-fetcher-error-consumer-endpoint", e =>
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
logger.LogInformation("YouTubeCommentsFetcher: SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("YouTubeCommentsFetcher: API KEY: {ApiKey}", youTubeConfig.ApiKey);
logger.LogInformation("YouTubeCommentsFetcher: RABBITMQ HOST: {RabbitMqHost}", rabbitMqConfig.Hostname);

await host.RunAsync();
