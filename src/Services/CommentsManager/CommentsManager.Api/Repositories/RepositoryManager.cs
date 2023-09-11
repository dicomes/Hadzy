using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;

namespace CommentsManager.Api.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICommentRepository> _commentRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _commentRepository = new Lazy<ICommentRepository>(() => new
            CommentRepository(repositoryContext));
    } 
    
    public ICommentRepository Comment => _commentRepository.Value;
    public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
}