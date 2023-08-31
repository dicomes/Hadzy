using CommentsFetchInfoManager.MinimalApi.Configurations;
using CommentsFetchInfoManager.MinimalApi.Exceptions;
using CommentsFetchInfoManager.MinimalApi.Extensions;
using CommentsFetchInfoManager.MinimalApi.Mapper;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;
using CommentsFetchInfoManager.MinimalApi.Repositories;
using CommentsFetchInfoManager.MinimalApi.Services;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;
using CommentsFetchInfoManager.MinimalApi.Validations;
using FluentValidation;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

var seqConfig = builder.Configuration.GetSection("Seq").Get<SeqConfig>();
var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>();
var mongoDbConfig = builder.Configuration.GetSection("MongoDb").Get<MongoDbConfig>();

{
    builder.UseSeqLogger(seqConfig);
    builder.Services.Configure<MongoDbConfig>(
        builder.Configuration.GetSection("MongoDb"));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(MappingConfig));
    builder.Services.AddTransient<IErrorResponseService, ErrorResponseService<FetchInfoDto>>();
    builder.Services.AddTransient<IFetchInfoHandlerService, FetchInfoHandlerService>();
    builder.Services.AddTransient<IFetchInfoRepository, MongoDbFetchInfoRepository>();
    builder.Services.AddTransient<IValidationService<FetchInfoDto>, FetchInfoValidationService>();
    builder.Services.AddTransient<IEventPublisherService, EventPublisherService>();
    builder.Services.AddTransient<IFetchStatusHandler, HandlerFetchedBefore>();
    builder.Services.AddTransient<IFetchStatusHandler, HandlerFailedBefore>();
    builder.Services.AddTransient<IFetchStatusHandler, HandlerInProgress>();
    builder.Services.AddTransient<IFetchStatusHandler, HandlerNewFetch>();
    builder.Services.AddTransient<IFetchInfoService, FetchInfoService>();
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();
    builder.Services.AddMassTransit(configurator =>
    {
        configurator.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(new Uri(rabbitMqConfig.Hostname), configure =>
            {
                configure.Username(rabbitMqConfig.User);
                configure.Password(rabbitMqConfig.Password);
            });
        });
    });
}

var app = builder.Build();
{
    app.UseCustomExceptionHandler();
    app.ConfigureEndpoints();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("CommentsFetchInfoManager service settings-------------------->");
logger.LogInformation("SEQ URL: {SeqUrl}", seqConfig.Url);
logger.LogInformation("RABBITMQ HOST: {RabbitMqHost}", rabbitMqConfig.Hostname);
logger.LogInformation("MONGODB CONNECTION: {MongoDbConnection}", mongoDbConfig.ConnectionString);
logger.LogInformation("<--------------------CommentsFetchInfoManager service settings");

app.Run();
