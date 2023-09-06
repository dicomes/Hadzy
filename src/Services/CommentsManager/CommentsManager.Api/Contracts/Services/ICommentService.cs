using System.Linq.Expressions;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Contracts.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetCommentsByExpressionAsync(Expression<Func<Comment, bool>> expression);
}