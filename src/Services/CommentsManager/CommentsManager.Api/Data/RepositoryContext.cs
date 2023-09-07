using CommentsManager.Api.Configurations;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CommentsManager.Api.Data;

public class RepositoryContext : DbContext
{
    public DbSet<Comment> Comments { get; set; }
    
    public RepositoryContext(DbContextOptions options)
        : base(options)
    {
    }
}