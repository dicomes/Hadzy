using CommentsFetchInfoIntegration.Worker;
using CommentsFetchInfoIntegration.Worker.Builders;
using CommentsFetchInfoIntegration.Worker.Configurations;
using CommentsFetchInfoIntegration.Worker.Consumers;
using CommentsFetchInfoIntegration.Worker.Mapper;
using CommentsFetchInfoIntegration.Worker.Repositories;
using CommentsFetchInfoIntegration.Worker.Services;
using CommentsFetchInfoIntegration.Worker.Services.Interfaces;
using GreenPipes;
using MassTransit;
using Serilog;

var seqConfig = new SeqConfig();
var rabbitMqConfig = new RabbitMqConfig();
var mongoDbConfig = new MongoDbConfig();

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
    
        seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
        rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
        mongoDbConfig = hostingContext.Configuration.GetSection("MongoDb").Get<MongoDbConfig>();
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig.Url)
            .WriteTo.Console()
            .CreateLogger();
        
        services.AddHostedService<Worker>();
        services.AddAutoMapper(typeof(MappingConfig));
        services.Configure<MongoDbConfig>(
            hostingContext.Configuration.GetSection("MongoDb"));
        services.AddSingleton<IFetchInfoRepository, MongoDbFetchInfoRepository>();
        services.AddSingleton<IFetchInfoService, FetchInfoService>();
        services.AddSingleton<IFetchInfoChangedEventHandler, FetchInfoChangedEventHandler>();
        services.AddSingleton<CommentsFetchInfoEventBuilder>();
        services.AddTransient<FetchInfoChangedEventConsumer>();

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                {
                    configure.Username(rabbitMqConfig.User);
                    configure.Password(rabbitMqConfig.Password);
                });
                
                cfg.ReceiveEndpoint("fetch-info-queue", e =>
                {
                    e.Consumer<FetchInfoChangedEventConsumer>(context);
                    e.UseScheduledRedelivery(r =>
                        r.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(3)));
                });
            });
        });
    })
    .UseSerilog()
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("CommentsFetchInfoIntegration service settings-------------------->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);
logger.LogInformation("MONGODB CONNECTION: {ApiKey}", mongoDbConfig.ConnectionString);
logger.LogInformation("<--------------------CommentsFetchInfoIntegration service settings");

await host.RunAsync();

