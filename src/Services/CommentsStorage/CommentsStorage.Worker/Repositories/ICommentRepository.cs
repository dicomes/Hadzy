using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Repositories;

public interface ICommentRepository
{
    Task AddAsync(Comment comment);
    Task AddAsync(IEnumerable<Comment> comments);
    Task<Comment?> GetByIdAsync(string id);
    Task<IEnumerable<Comment>> GetAllByVideoIdAsync(string videoId);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(string id);
}