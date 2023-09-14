using CommentsStorage.Worker.Contracts.Repositories;
using CommentsStorage.Worker.Data;
using CommentsStorage.Worker.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsStorage.Worker.Repositories;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    public CommentRepository(RepositoryContext repositoryContext) :
        base(repositoryContext)
    {
    }

    public async Task<Comment> GetByIdAsync(string id, bool trackChanges) => 
        await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

    public async Task<IEnumerable<Comment>> GetByVideoIdAsync(string videoId, bool trackChanges) =>
        await FindByCondition(c => c.VideoId == videoId, trackChanges)
            .ToListAsync();
    
    public async Task CreateComment(Comment comment)
    {
        Create(comment);
        await RepositoryContext.SaveChangesAsync();
    }

    public async Task CreateComments(IEnumerable<Comment> comments)
    {
        CreateRange(comments);
        await RepositoryContext.SaveChangesAsync();
    }

    public async Task UpdateComment(Comment comment)
    {
        Update(comment);
        await RepositoryContext.SaveChangesAsync();
    }

    public async Task DeleteComment(Comment comment)
    {
        Delete(comment);
        await RepositoryContext.SaveChangesAsync();
    }
}