namespace CommentsManager.Api.Contracts.Repositories;

public interface IRepositoryManager
{
    ICommentRepository Comment { get; }
    Task SaveAsync();
}