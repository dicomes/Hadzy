using CommentsStorage.Worker.Contracts.Repositories;
using CommentsStorage.Worker.Contracts.Services;
using CommentsStorage.Worker.Models;
using CommentsStorage.Worker.Repositories;

namespace CommentsStorage.Worker.Services;

public class CommentService : ICommentService
{
    private readonly IRepositoryManager _repository;

    public CommentService(
        IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _repository.Comment.CreateComment(comment);
    }

    public async Task AddCommentsAsync(IEnumerable<Comment> comments)
    {
        await _repository.Comment.CreateComments(comments);
    }

    public async Task<Comment?> GetByIdAsync(string id, bool trackChanges)
    {
        return await _repository.Comment.GetByIdAsync(id, trackChanges:false);
    }

    public async Task<IEnumerable<Comment>> GetByVideoIdAsync(string videoId, bool trackChanges)
    {
        return await _repository.Comment.GetByVideoIdAsync(videoId, trackChanges: false);
    }

    public async Task UpdateAsync(Comment comment)
    {
        await _repository.Comment.UpdateComment(comment);
    }

    public async Task DeleteAsync(string id)
    {
        var comment = await _repository.Comment.GetByIdAsync(id, trackChanges: true);
        if (comment != null)
        {
            await _repository.Comment.DeleteComment(comment);
        }
    }
}
