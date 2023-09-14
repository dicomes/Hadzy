using CommentsManager.Api.Contracts.Exceptions;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.Contracts.Utilities;
using CommentsManager.Api.Extensions;
using CommentsManager.Api.Mapping;
using CommentsManager.Api.Repositories;
using CommentsManager.Api.Services;
using CommentsManager.Api.Utilities;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLogging();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure services
builder.Services.ConfigureCors();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Register services to the container.
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddTransient<ISanitizationService, SanitizationService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();
builder.Services.AddControllers(config => {
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
});

var app = builder.Build();

// Exception Handling
app.UseExceptionHandlerService();

// Development Tools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Core Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

// Routing and Controllers
app.MapControllers();

app.Run();
