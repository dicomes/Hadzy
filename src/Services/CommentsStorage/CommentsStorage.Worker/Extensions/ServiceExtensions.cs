using CommentsStorage.Worker.Contracts.Repositories;
using CommentsStorage.Worker.Data;
using CommentsStorage.Worker.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CommentsStorage.Worker.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // PostgreSQL
        services.AddDbContext<RepositoryContext>(options =>
            options.UseNpgsql(configuration["Database:ConnectionString"]));
    }
}