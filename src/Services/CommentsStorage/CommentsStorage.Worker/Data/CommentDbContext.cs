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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Id).IsRequired(true);
            entity.Property(e => e.Etag).IsRequired(false);
            entity.Property(e => e.AuthorDisplayName).IsRequired(false);
            entity.Property(e => e.AuthorProfileImageUrl).IsRequired(false);
            entity.Property(e => e.AuthorChannelUrl).IsRequired(false);
            entity.Property(e => e.AuthorChannelId).IsRequired(false);
            entity.Property(e => e.ChannelId).IsRequired(false);
            entity.Property(e => e.VideoId).IsRequired(true);
            entity.Property(e => e.TextDisplay).IsRequired(false);
            entity.Property(e => e.TextOriginal).IsRequired(false);
            entity.Property(e => e.ViewerRating).IsRequired(false);
            entity.Property(e => e.LikeCount).IsRequired(true); 
            entity.Property(e => e.PublishedAt).IsRequired(true);
            entity.Property(e => e.UpdatedAt).IsRequired(true);
            entity.Property(e => e.TotalReplyCount).IsRequired(true);
        });
    }

}