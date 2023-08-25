using CommentsStorage.Worker.Data;
using CommentsStorage.Worker.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsStorage.Worker.Repositories;

public class PostgreCommentRepository : ICommentRepository
{
    private readonly CommentDbContext _context;

    public PostgreCommentRepository(CommentDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(IEnumerable<Comment> comments)
    {
        await _context.Comments.AddRangeAsync(comments);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment?> GetByIdAsync(string id)
    {
        return await _context.Comments.FindAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetAllByVideoIdAsync(string videoId)
    {
        return await _context.Comments.Where(c => c.VideoId == videoId).ToListAsync();
    }

    public async Task UpdateAsync(Comment comment)
    {
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment != null)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
