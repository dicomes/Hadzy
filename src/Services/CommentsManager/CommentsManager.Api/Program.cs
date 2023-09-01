using CommentsManager.Api.Configurations;
using CommentsManager.Api.Data;
using CommentsManager.Api.Extensions;
using CommentsManager.Api.Mapping;
using CommentsManager.Api.Repositories;
using CommentsManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var seqConfig = builder.Configuration.GetSection("Seq").Get<SeqConfig>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureCors();
builder.ConfigureLogging();
builder.ConfigurePostgreSqlSettings();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddDbContext<CommentDbContext>();
builder.Services.AddScoped<ICommentRepository, PostgreCommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
