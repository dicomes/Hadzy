using System.Linq.Expressions;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Repositories;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IEnumerable<Comment>> FindByConditionAsync(Expression<Func<Comment, bool>> expression);
}