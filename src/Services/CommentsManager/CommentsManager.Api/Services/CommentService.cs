using AutoMapper;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.Models;
using CommentsManager.Api.RequestParameters;

namespace CommentsManager.Api.Services;

public class CommentService : ICommentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CommentService(
        IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedList<CommentResponse>> GetCommentsPageByQueryAsync(
        string videoId, CommentsParameters parameters)
    {
        (IEnumerable<Comment> comments, var totalCount) = await _repository.Comment.GetByVideoIdAndQueryAsync(videoId, parameters, false);
        
        var commentsResponse = _mapper.Map<List<CommentResponse>>(comments);
        
        return new PagedList<CommentResponse>(
            commentsResponse, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    private async Task CheckIfCommentsExists(string videoId)
    {
        var comments = await _repository.Comment.GetByVideoIdAsync(videoId, false);
        if (comments == null)
        {
            throw new CommentNotFoundException(videoId);
        }
    }
}