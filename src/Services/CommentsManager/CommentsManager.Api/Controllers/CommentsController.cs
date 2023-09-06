using System.Linq.Expressions;
using AutoMapper;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.Models;
using CommentsManager.Api.Repositories;
using CommentsManager.Api.Services;
using CommentsManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CommentsManager.Api.Controllers;

// [ApiController]
[Route("comments-manager/api/v1/video/{videoId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentService _commentService;
    private readonly IMapper _mapper;

    public CommentsController(
        ILogger<CommentsController> logger,
        ICommentService commentService,
        IMapper mapper)
    {
        _logger = logger;
        _commentService = commentService;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetComments")]
    public async Task<IActionResult> GetCommentsFiltered([FromRoute] string videoId, [FromQuery] GetComment getComment)
    {
        if (!ModelState.IsValid)
        {
            throw new ModelValidationException(ModelState);
        }
        
        Expression<Func<Comment, bool>> filterExpression = comment => comment.VideoId == videoId;

        IEnumerable<GetCommentResponse> commentsResponse = await _commentService.GetCommentsByExpressionAsync(filterExpression);
            
        return Ok(commentsResponse);
    }
}
