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
            
            // Vectors
            entity.Property(e => e.TextDisplaySearchVector)
                .HasComputedColumnSql("to_tsvector('simple', coalesce(\"TextDisplay\", ''))", stored: true);
            entity.Property(e => e.AuthorDisplayNameSearchVector)
                .HasComputedColumnSql("to_tsvector('simple', coalesce(\"AuthorDisplayName\", ''))", stored: true);

            // Indexes
            entity.HasIndex(e => e.VideoId).HasDatabaseName("IX_Comments_VideoId");
            entity.HasIndex(e => e.PublishedAt).HasDatabaseName("IX_Comments_PublishedAt");

            // Composite Index for VideoId and PublishedAt
            entity.HasIndex(e => new { e.VideoId, e.PublishedAt })
                .HasDatabaseName("IX_Comments_VideoId_PublishedAt");

            // Indexes for tsvector columns
            entity.HasIndex(e => e.TextDisplaySearchVector).HasDatabaseName("IX_Comments_TextDisplaySearchVector")
                .HasMethod("GIN");
            entity.HasIndex(e => e.AuthorDisplayNameSearchVector).HasDatabaseName("IX_Comments_AuthorDisplayNameSearchVector")
                .HasMethod("GIN");
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
}
