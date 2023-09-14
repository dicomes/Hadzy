namespace CommentsManager.Api.Contracts.Repositories;

public interface IRepositoryManager
{
    ICommentRepository Comment { get; }
    IVideoRepository Video { get; }
    Task SaveAsync();
}