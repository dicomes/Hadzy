using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;

namespace CommentsManager.Api.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICommentRepository> _commentRepository;
    private readonly Lazy<IVideoRepository> _videoRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _commentRepository = new Lazy<ICommentRepository>(() => new
            CommentRepository(repositoryContext));
        _videoRepository = new Lazy<IVideoRepository>(() => new
            VideoRepository(repositoryContext));
    } 
    
    public ICommentRepository Comment => _commentRepository.Value;
    public IVideoRepository Video => _videoRepository.Value;
    public Task SaveAsync() => _repositoryContext.SaveChangesAsync();
}