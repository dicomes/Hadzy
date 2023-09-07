
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Contracts.Repositories;

public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(string videoId, bool trackChanges);
    Task<IEnumerable<Comment>> GetByVideoIdAsync(string videoId, bool trackChanges);
}