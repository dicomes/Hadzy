using System.Linq.Expressions;
using CommentsStorage.Worker.Contracts.Repositories;
using CommentsStorage.Worker.Data;
using Microsoft.EntityFrameworkCore;

namespace CommentsStorage.Worker.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RepositoryContext RepositoryContext;
    protected RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ?
        RepositoryContext.Set<T>().AsNoTracking() :
        RepositoryContext.Set<T>();
    
    public IQueryable<T> FindByCondition(
        Expression<Func<T, bool>> expression, bool trackChanges) => !trackChanges ?
        RepositoryContext.Set<T>().Where(expression).AsNoTracking() :
        RepositoryContext.Set<T>().Where(expression);
    
    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    
    public void CreateRange(IEnumerable<T> entities) => RepositoryContext.Set<T>().AddRange(entities);
}