using CommentsManager.Api.Models;

namespace CommentsManager.Api.Services;

public interface ICommentService
{
    Task<Comment> GetCommentByIdAsync(string id);
    Task<IEnumerable<Comment>> GetAllCommentsByVideoIdAsync(string videoId);
}