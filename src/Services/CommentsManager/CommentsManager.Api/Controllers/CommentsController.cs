using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Controllers;

// [ApiController]
[Route("comments-manager/api/v1/video/{videoId}/comments")]
public class CommentsController : ControllerBase
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentService _commentService;

    public CommentsController(
        ILogger<CommentsController> logger,
        ICommentService commentService)
    {
        _logger = logger;
        _commentService = commentService;
    }

    [ProducesResponseType(typeof(ApiResponse<GetCommentResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse<GetCommentResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiResponse<GetCommentResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Consumes("application/json")]
    [Produces("application/json")]
    [HttpGet(Name = "GetComments")]
    public async Task<IActionResult> GetCommentsFiltered([FromRoute] string videoId, [FromQuery] GetComment getComment)
    {
        if (!ModelState.IsValid)
        {
            throw new ModelValidationException(ModelState);
        }
        
        Expression<Func<Comment, bool>> filterExpression = comment => comment.VideoId == videoId;
        IEnumerable<CommentResponse> commentsResponse = await _commentService.GetCommentsByExpressionAsync(filterExpression);

        if (!commentsResponse.Any())
        {
            throw new CommentNotFoundException(videoId);
        }
            
        return Ok(commentsResponse);
    }
}
