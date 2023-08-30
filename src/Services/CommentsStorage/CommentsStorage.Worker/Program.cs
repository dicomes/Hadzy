using CommentsStorage.Worker;
using CommentsStorage.Worker.Configurations;
using CommentsStorage.Worker.Consumers;
using CommentsStorage.Worker.Data;
using CommentsStorage.Worker.Mapper;
using CommentsStorage.Worker.Repositories;
using CommentsStorage.Worker.Services;
using MassTransit;
using Serilog;

var seqConfig = new SeqConfig();
var rabbitMqConfig = new RabbitMqConfig();
var postgreSqlConfig = new PostgreSqlConfig();

Microsoft.Extensions.Hosting.IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostingContext, services) =>
    {
    
        seqConfig = hostingContext.Configuration.GetSection("Seq").Get<SeqConfig>();
        rabbitMqConfig = hostingContext.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
        postgreSqlConfig = hostingContext.Configuration.GetSection("PostgreSql").Get<PostgreSqlConfig>();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig.Url)
            .WriteTo.Console()
            .CreateLogger();
        
        services.Configure<PostgreSqlConfig>(
            hostingContext.Configuration.GetSection("PostgreSql"));
        services.AddHostedService<Worker>();
        services.AddAutoMapper(typeof(MappingConfig));
        services.AddDbContext<CommentDbContext>();
        services.AddTransient<IIntegrationService, IntegrationService>();
        services.AddScoped<ICommentRepository, PostgreCommentRepository>();
        services.AddScoped<ICommentService, CommentService>();
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
    })
    .UseSerilog()
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("CommentsStorage service settings-------------------->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("RABBITMQ HOST: {ApiKey}", rabbitMqConfig.Hostname);
logger.LogInformation("POSTGRESQL HOST: {ApiKey}", postgreSqlConfig.ConnectionString);
logger.LogInformation("<--------------------CommentsStorage service settings");

await host.RunAsync();

