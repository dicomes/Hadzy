using CommentsManager.Api.Exceptions;
using CommentsManager.Api.Extensions;
using CommentsManager.Api.Mapping;
using CommentsManager.Api.Repositories;
using CommentsManager.Api.Services;
using CommentsManager.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureLogging();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure services
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureCors();

// Register services to the container.
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddScoped<ICommentRepository, PostgresCommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IErrorResponseService, ErrorResponseService>();
builder.Services.AddScoped<IExceptionHandlerService, ExceptionHandlerService>();
builder.Services.AddControllers();

var app = builder.Build();

// Exception Handling
app.UseCustomExceptionHandler();

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
