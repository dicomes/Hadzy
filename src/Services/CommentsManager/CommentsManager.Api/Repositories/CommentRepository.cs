using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Data;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Enums;
using CommentsManager.Api.Extensions;
using CommentsManager.Api.Mapping;
using CommentsManager.Api.Models;
using CommentsManager.Api.RequestParameters;
using Microsoft.EntityFrameworkCore;

namespace CommentsManager.Api.Repositories;

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

    public async Task<(IEnumerable<Comment>, int totalCount)> GetByVideoIdAndQueryAsync(
        string videoId, CommentsParameters parameters, bool trackChanges)
    {
        if (!CommentsOrderMapping.Criteria.TryGetValue(parameters.OrderBy, out var orderExpression))
        {
            throw new ArgumentException($"No order criteria exists for {parameters.OrderBy}", nameof(parameters.OrderBy));
        }

        var query = FindByCondition(c => c.VideoId == videoId, trackChanges);

        if (!string.IsNullOrEmpty(parameters.SearchTerm))
        {
            query = query.SearchByText(parameters.SearchTerm);
        }
    
        if (!string.IsNullOrEmpty(parameters.Author))
        {
            query = query.SearchByAuthor(parameters.Author);
        }

        if (parameters.Direction == Direction.Ascending)
        {
            query = query.OrderBy(orderExpression);
        }
        else
        {
            query = query.OrderByDescending(orderExpression);
        }

        // Include pagination
        var count = await query.CountAsync();
        var comments = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();

        return (comments, count);
    }

}