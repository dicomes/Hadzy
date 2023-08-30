using CommentsStorage.Worker.Models;
using CommentsStorage.Worker.Repositories;

namespace CommentsStorage.Worker.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task AddCommentAsync(Comment comment)
    {
        await _commentRepository.AddAsync(comment);
    }

    public async Task AddCommentsAsync(IEnumerable<Comment> comments)
    {
        await _commentRepository.AddAsync(comments);
    }

    public async Task<Comment?> GetCommentByIdAsync(string id)
    {
        return await _commentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsByVideoIdAsync(string videoId)
    {
        return await _commentRepository.GetAllByVideoIdAsync(videoId);
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        await _commentRepository.UpdateAsync(comment);
    }

    public async Task DeleteCommentAsync(string id)
    {
        await _commentRepository.DeleteAsync(id);
    }
}
