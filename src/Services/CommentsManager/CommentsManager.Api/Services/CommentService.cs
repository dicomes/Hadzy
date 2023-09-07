using System.Linq.Expressions;
using AutoMapper;
using CommentsManager.Api.Contracts.Repositories;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.Factories;
using CommentsManager.Api.Models;

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

    public async Task<CommentsPageResponse> GetCommentsPageByQueryAsync(
        string videoId, QueryForCommentsPage queryForCommentsPage)
    {

        var comments = await _repository.Comment.GetByVideoIdAsync(videoId, false);
        
        if (!comments.Any())
        {
            throw new CommentNotFoundException(videoId);
        }
        
        var commentsResponse = _mapper.Map<List<CommentForResponse>>(comments);
        var response = CommentsPageResponseFactory.Create(
            commentsResponse, queryForCommentsPage, commentsResponse.Count);
        
        return response;
    }
    
}