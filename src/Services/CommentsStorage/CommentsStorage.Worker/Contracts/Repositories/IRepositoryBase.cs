using System.Linq.Expressions;
using CommentsStorage.Worker.Data;

namespace CommentsStorage.Worker.Contracts.Repositories;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    public void CreateRange(IEnumerable<T> entities);
}