using AutoMapper;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.Contracts.Utilities;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;
using CommentsManager.Api.RequestParameters;

namespace CommentsManager.Api.Services;

public class CommentService : ICommentService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CommentService> _logger;
    private readonly ISanitizationService _sanitizationService;


    public CommentService(
        IRepositoryManager repository,
        IMapper mapper,
        ILogger<CommentService> logger,
        ISanitizationService sanitizationService)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _sanitizationService = sanitizationService;
    }

    public async Task<PagedList<CommentResponse>> GetCommentsPageByQueryAsync(
        string videoId, CommentsParameters parameters)
    {
        _logger.LogInformation("{Source}: GetCommentsPageByQueryAsync for VideoId {VideoId}. Params: {Params}",
            GetType().Name, videoId, parameters.ToString());

        parameters.SearchTerm = _sanitizationService.SanitizeSearchTerm(parameters.SearchTerm);
        parameters.Author = _sanitizationService.SanitizeSearchTerm(parameters.Author);

        (IEnumerable<Comment> comments, var totalCount) = await _repository.Comment.GetByVideoIdAndQueryAsync(videoId, parameters, false);
        
        var commentsResponse = _mapper.Map<List<CommentResponse>>(comments);
        
        return new PagedList<CommentResponse>(
            commentsResponse, totalCount, parameters.PageNumber, parameters.PageSize);
    }
    
    public async Task<CommentResponse> GetCommentByIdAsync(
        string videoId)
    {
        _logger.LogInformation("{Source}: GetCommentByVideoIdAsync for VideoId {VideoId}.",
            GetType().Name, videoId);
        
        Comment comment = await _repository.Comment.GetByIdAsync(videoId, false);
        var commentsResponse = _mapper.Map<CommentResponse>(comment);
        
        return commentsResponse;
    }
}