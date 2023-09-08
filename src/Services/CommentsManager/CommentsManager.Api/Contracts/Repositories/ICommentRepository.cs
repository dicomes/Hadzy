
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;
using CommentsManager.Api.RequestParameters;

namespace CommentsManager.Api.Contracts.Repositories;

public interface ICommentRepository
{
    Task<Comment> GetByIdAsync(string videoId, bool trackChanges);
    Task<IEnumerable<Comment>> GetByVideoIdAsync(
        string videoId, bool trackChanges);
    Task<(IEnumerable<Comment>, int totalCount)> GetByVideoIdAndQueryAsync(
        string videoId, CommentsParameters parameters, bool trackChanges);
}