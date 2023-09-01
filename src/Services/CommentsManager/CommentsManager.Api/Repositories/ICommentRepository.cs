using CommentsManager.Api.Models;

namespace CommentsManager.Api.Repositories;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(string id);
    Task<IEnumerable<Comment>> GetAllByVideoIdAsync(string id);
}