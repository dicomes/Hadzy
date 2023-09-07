using CommentsManager.Api.Configurations;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;
using CommentsManager.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CommentsManager.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL
        services.AddDbContext<RepositoryContext>(options =>
            options.UseNpgsql(configuration["Database:ConnectionString"]));
    }
}