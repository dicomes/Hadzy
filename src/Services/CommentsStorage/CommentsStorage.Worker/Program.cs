using CommentsStorage.Worker;
using CommentsStorage.Worker.Configurations;
using CommentsStorage.Worker.Consumers;
using CommentsStorage.Worker.Contracts.Repositories;
using CommentsStorage.Worker.Contracts.Services;
using CommentsStorage.Worker.Data;
using CommentsStorage.Worker.Extensions;
using CommentsStorage.Worker.Mapper;
using CommentsStorage.Worker.Repositories;
using CommentsStorage.Worker.Services;
using GreenPipes;
using MassTransit;
using Serilog;

var seqConfig = new SeqConfig();
var rabbitMqConfig = new RabbitMqConfig();

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
        seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
        rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig.Url)
            .WriteTo.Console()
            .CreateLogger();
        
        services.ConfigureDbContext(hostingContext.Configuration);
        services.AddHostedService<Worker>();
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddScoped<IIntegrationService, IntegrationService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddTransient<CommentThreadListCompletedEventConsumer>();
        services.AddTransient<FetchStartedEventConsumer>();
        services.AddTransient<FetchCompletedEventConsumer>();

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                {
                    configure.Username(rabbitMqConfig.User);
                    configure.Password(rabbitMqConfig.Password);
                });
                
                cfg.ReceiveEndpoint("comment-storage-thread-consumer-endpoint", e =>
                {
                    e.Consumer<CommentThreadListCompletedEventConsumer>(context);
                    //First Retry: Delay = 100ms + (2^1 - 1) * 100ms = 200ms
                    //Second Retry: Delay = 100ms + (2^2 - 1) * 100ms = 300ms
                    //Third Retry: Delay = 100ms + (2^3 - 1) * 100ms = 700ms
                    e.UseMessageRetry(r =>
                        r.Exponential(3, TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(100)));
                });
                
                cfg.ReceiveEndpoint("comment-storage-fetch-started-consumer-endpoint", e =>
                {
                    e.Consumer<FetchStartedEventConsumer>(context);
                    //First Retry: Delay = 100ms + (2^1 - 1) * 100ms = 200ms
                    //Second Retry: Delay = 100ms + (2^2 - 1) * 100ms = 300ms
                    //Third Retry: Delay = 100ms + (2^3 - 1) * 100ms = 700ms
                    e.UseMessageRetry(r =>
                        r.Exponential(3, TimeSpan.FromMilliseconds(100),
                            TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(100)));
                });
                cfg.ReceiveEndpoint("comment-storage-fetch-completed-consumer-endpoint", e =>
                {
                    e.Consumer<FetchCompletedEventConsumer>(context);
                    //First Retry: Delay = 100ms + (2^1 - 1) * 100ms = 200ms
                    //Second Retry: Delay = 100ms + (2^2 - 1) * 100ms = 300ms
                    //Third Retry: Delay = 100ms + (2^3 - 1) * 100ms = 700ms
                    e.UseMessageRetry(r =>
                        r.Exponential(3, TimeSpan.FromMilliseconds(100),
                            TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(100)));
                });
            });
        });
        
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
            dbContext.EnsureDatabaseMigrated();
        }
    })
    .UseSerilog()
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("CommentsStorage: SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("CommentsStorage: RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);

await host.RunAsync();

