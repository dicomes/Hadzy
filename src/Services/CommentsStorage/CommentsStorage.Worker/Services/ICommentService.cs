using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Services;

public interface ICommentService
{
    Task AddCommentAsync(Comment comment);
    Task AddCommentsAsync(IEnumerable<Comment> comments);
    Task<Comment?> GetCommentByIdAsync(string id);
    Task<IEnumerable<Comment>> GetAllCommentsByVideoIdAsync(string videoId);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(string id);
}