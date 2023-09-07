using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Repositories;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<Comment> GetByIdAsync(string id, bool trackChanges) => 
        await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
    
    public async Task<IEnumerable<Comment>> GetByVideoIdAsync(string videoId, bool trackChanges) =>
        await FindByCondition(x => x.VideoId == videoId, trackChanges)
            .ToListAsync();
        
}