using System.Linq.Expressions;

namespace CommentsManager.Api.Data;

public interface IDataAccessAdapter<TEntity>
{
    Task<TEntity?> GetByIdAsync(object id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(object id);
    Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> expression);
}