using System.Linq.Expressions;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Services.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<GetCommentResponse>> GetCommentsByExpressionAsync(Expression<Func<Comment, bool>> expression);
}