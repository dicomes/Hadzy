using System.Linq.Expressions;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Services;

public interface ICommentService
{
    Task<IEnumerable<Comment>> FindByConditionAsync(Expression<Func<Comment, bool>> expression);
}