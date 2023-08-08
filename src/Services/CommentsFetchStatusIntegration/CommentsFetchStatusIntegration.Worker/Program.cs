using CommentsFetchStatusIntegration.Worker;
using CommentsFetchStatusIntegration.Worker.Configurations;
using CommentsFetchStatusIntegration.Worker.Consumers;
using CommentsFetchStatusIntegration.Worker.Mapper;
using CommentsFetchStatusIntegration.Worker.Services;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
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
        
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig.Url)
            .WriteTo.Console()
            .CreateLogger();
        
        services.AddHostedService<Worker>();
        
        services.AddAutoMapper(typeof(MappingConfig)); 
        services.Configure<MongoDbConfig>(
            hostingContext.Configuration.GetSection("MongoDb"));
        services.AddSingleton<IVideoCommentsStatusService, VideoCommentsStatusService>();
        services.AddTransient<CommentsFetchStatusEventConsumer>();

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                {
                    configure.Username(rabbitMqConfig.User);
                    configure.Password(rabbitMqConfig.Password);
                });
                
                cfg.ReceiveEndpoint("fetch-status-queue", e =>
                {
                    e.Consumer<CommentsFetchStatusEventConsumer>(context);
                });
            });
        });
    })
    .UseSerilog()
    .Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("CommentsFetchStatusIntergration service settings--->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);
logger.LogInformation("<---CommentsFetchStatusIntergration service settings");
await host.RunAsync();

