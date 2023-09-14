using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Data;

public class RepositoryContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Video> Videos { get; set; }
    
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }
}