using CommentsManager.Api.Configurations;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CommentsManager.Api.Data;

public class CommentDbContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    
    public CommentDbContext(DbContextOptions<CommentDbContext> options)
        : base(options)
    {
    }
}