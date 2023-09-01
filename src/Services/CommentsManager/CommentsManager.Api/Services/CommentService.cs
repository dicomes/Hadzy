using CommentsManager.Api.Models;
using CommentsManager.Api.Repositories;

namespace CommentsManager.Api.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Comment> GetCommentByIdAsync(string id)
    {
        return await _commentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Comment>> GetAllCommentsByVideoIdAsync(string videoId)
    {
        return await _commentRepository.GetAllByVideoIdAsync(videoId);
    }
    
}