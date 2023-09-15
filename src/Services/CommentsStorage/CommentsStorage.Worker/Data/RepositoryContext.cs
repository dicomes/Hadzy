using CommentsStorage.Worker.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsStorage.Worker.Data;

public class RepositoryContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Video> Videos { get; set; }

    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            // Property configurations
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.Etag).IsRequired(false);
            entity.Property(e => e.AuthorDisplayName).IsRequired(false);
            entity.Property(e => e.AuthorProfileImageUrl).IsRequired(false);
            entity.Property(e => e.AuthorChannelUrl).IsRequired(false);
            entity.Property(e => e.AuthorChannelId).IsRequired(false);
            entity.Property(e => e.ChannelId).IsRequired(false);
            entity.Property(e => e.VideoId).IsRequired();
            entity.Property(e => e.TextDisplay).IsRequired(false);
            entity.Property(e => e.TextOriginal).IsRequired(false);
            entity.Property(e => e.ViewerRating).IsRequired(false);
            entity.Property(e => e.LikeCount).IsRequired();
            entity.Property(e => e.PublishedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.Property(e => e.TotalReplyCount).IsRequired();
            
            // Set relationship
            entity.HasOne<Video>() 
                .WithMany()
                .HasForeignKey(c => c.VideoId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Set up generated tsvector columns
            entity.HasGeneratedTsVectorColumn(
                    e => e.TextDisplaySearchVector,
                    "simple", 
                    e => new { e.TextDisplay })
                .HasIndex(e => e.TextDisplaySearchVector)
                .HasMethod("GIN");

            entity.HasGeneratedTsVectorColumn(
                    e => e.AuthorDisplayNameSearchVector,
                    "simple",
                    e => new { e.AuthorDisplayName })
                .HasIndex(e => e.AuthorDisplayNameSearchVector)
                .HasMethod("GIN");

            // Indexes
            entity.HasIndex(e => e.VideoId).HasDatabaseName("IX_Comments_VideoId");
            entity.HasIndex(e => e.PublishedAt).HasDatabaseName("IX_Comments_PublishedAt");

            // Composite Index for VideoId and PublishedAt
            entity.HasIndex(e => new { e.VideoId, e.PublishedAt })
                .HasDatabaseName("IX_Comments_VideoId_PublishedAt");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            // Property configurations
            entity.Property(e => e.Id).IsRequired();
            entity.Property(e => e.FirstComment).IsRequired(false);

            // Indexes
            entity.HasIndex(e => e.Id).HasDatabaseName("IX_Videos_Id");
        });
    }
    
    public void EnsureDatabaseMigrated()
    {
        // This will apply all pending migrations. 
        // If the database doesn't exist, it will be created and all migrations will be applied.
        Database.Migrate();
    }
}
