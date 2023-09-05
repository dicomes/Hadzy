using System.Linq.Expressions;
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

    public async Task<IEnumerable<Comment>> FindByConditionAsync(Expression<Func<Comment, bool>> expression)
    {
        return await _commentRepository.FindByConditionAsync(expression);
    }
    
    
}