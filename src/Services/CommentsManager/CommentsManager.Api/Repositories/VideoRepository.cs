using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;
using CommentsManager.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Repositories;

public class VideoRepository : RepositoryBase<Video>, IVideoRepository
{
    public VideoRepository(RepositoryContext repositoryContext) :
        base(repositoryContext)
    {
    }

    public async Task<Video> GetByIdAsync(string id, bool trackChanges) => 
        await FindByCondition(c => c.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
}