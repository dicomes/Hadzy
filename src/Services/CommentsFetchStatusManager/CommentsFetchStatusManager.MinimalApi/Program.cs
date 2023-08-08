using CommentsFetchStatus.MinimalApi.Configurations;
using CommentsFetchStatus.MinimalApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var seqConfig = builder.Configuration.GetSection("Seq").Get<SeqConfig>();
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();

// Service registration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);

app.UseCustomExceptionHandler();

app.Run();
