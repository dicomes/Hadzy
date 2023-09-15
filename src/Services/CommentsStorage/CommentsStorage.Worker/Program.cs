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
        services.AddTransient<IIntegrationService, IntegrationService>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddTransient<CommentThreadListCompletedEventConsumer>();

        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
                {
                    configure.Username(rabbitMqConfig.User);
                    configure.Password(rabbitMqConfig.Password);
                });
                
                cfg.ReceiveEndpoint("comment-thread-queue", e =>
                {
                    e.Consumer<CommentThreadListCompletedEventConsumer>(context);
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
logger.LogInformation("CommentsStorage service settings-------------------->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);
logger.LogInformation("<--------------------CommentsStorage service settings");

await host.RunAsync();

