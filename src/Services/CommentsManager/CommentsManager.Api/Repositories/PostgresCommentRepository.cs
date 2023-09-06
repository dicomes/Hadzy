using System.Linq.Expressions;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CommentsManager.Api.Repositories;

public class PostgresCommentRepository : ICommentRepository
{
    private readonly CommentDbContext _context;

    public PostgresCommentRepository(CommentDbContext context)
    {
        _context = context;
    }

    public async Task<Comment?> GetByIdAsync(object id)
    {
        return await _context.Set<Comment>().FindAsync(id);
    }

    public async Task<Comment> InsertAsync(Comment entity)
    {
        await _context.Set<Comment>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Comment entity)
    {
        _context.Set<Comment>().Update(entity);
        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<bool> DeleteAsync(object id)
    {
        var entity = await _context.Set<Comment>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Set<Comment>().Remove(entity);
        return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<IEnumerable<Comment>> FindByConditionAsync(Expression<Func<Comment, bool>> expression)
    {
        return await _context.Set<Comment>().Where(expression).ToListAsync();
    }
    
}