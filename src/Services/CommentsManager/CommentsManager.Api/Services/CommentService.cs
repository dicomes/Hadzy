using System.Linq.Expressions;
using AutoMapper;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;
using CommentsManager.Api.Repositories;

namespace CommentsManager.Api.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public CommentService(
        ICommentRepository commentRepository,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<Comment> GetCommentByIdAsync(string id)
    {
        return await _commentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentsByExpressionAsync(Expression<Func<Comment, bool>> expression)
    {
        var comments = await _commentRepository.FindByConditionAsync(expression);
        return _mapper.Map<IEnumerable<CommentResponse>>(comments);
    }
}