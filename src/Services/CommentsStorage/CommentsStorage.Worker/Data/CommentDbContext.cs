using CommentsStorage.Worker.Configurations;
using CommentsStorage.Worker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CommentsStorage.Worker.Data;

public class CommentDbContext : DbContext
{
    private readonly IOptions<PostgreSqlConfig> _postgresSqlConfig;
    public DbSet<Comment> Comments { get; set; }
    
    public CommentDbContext(
        IOptions<PostgreSqlConfig> postgresSqlConfig)
    {
        _postgresSqlConfig = postgresSqlConfig;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_postgresSqlConfig.Value.ConnectionString);
    }
}