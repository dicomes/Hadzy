using CommentsStorage.Worker.Models;

namespace CommentsStorage.Worker.Contracts.Repositories;

public interface ICommentRepository
{
    Task<Comment?> GetByIdAsync(string videoId, bool trackChanges);
    Task<IEnumerable<Comment>> GetByVideoIdAsync(string videoId, bool trackChanges);
    Task CreateComment(Comment comment);
    Task CreateComments(IEnumerable<Comment> comments);
    Task UpdateComment(Comment comment);
    Task DeleteComment(Comment comment);
}