using CommentsManager.Api.Data;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Repositories;

public class PostgreCommentRepository : ICommentRepository
{
    private readonly CommentDbContext _context;

    public PostgreCommentRepository(CommentDbContext context)
    {
        _context = context;
    }
    
    public async Task<Comment?> GetByIdAsync(string id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetAllByVideoIdAsync(string id)
    {
        return await _context.Comments.Where(c => c.VideoId == id).ToListAsync();
    }
    
}