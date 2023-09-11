using AutoMapper;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;
using CommentsManager.Api.RequestParameters;

namespace CommentsManager.Api.Services;

public class CommentService : ICommentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentService> _logger;

    public CommentService(
        IRepositoryManager repository,
        IMapper mapper, ILogger<CommentService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<CommentResponse>> GetCommentsPageByQueryAsync(
        string videoId, CommentsParameters parameters)
    {
        _logger.LogInformation("{Source}: GetCommentsPageByQueryAsync for VideoId {VideoId}. Params: {Params}",
            GetType().Name, videoId, parameters.ToString());
        
        (IEnumerable<Comment> comments, var totalCount) = await _repository.Comment.GetByVideoIdAndQueryAsync(videoId, parameters, false);
        
        var commentsResponse = _mapper.Map<List<CommentResponse>>(comments);
        
        return new PagedList<CommentResponse>(
            commentsResponse, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}