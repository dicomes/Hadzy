using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Contracts.Services;

public interface ICommentService
{
    Task AddCommentAsync(Comment comment);
    Task AddCommentsAsync(IEnumerable<Comment> comments);
    Task<Comment?> GetByIdAsync(string id, bool trackChanges);
    Task<IEnumerable<Comment>> GetByVideoIdAsync(string videoId, bool trackChanges);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(string id);
}