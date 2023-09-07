using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Controllers;

[ApiController]
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

    [ProducesResponseType(typeof(ApiResponse<CommentsPageResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse<CommentsPageResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiResponse<CommentsPageResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Produces("application/json")]
    [HttpGet(Name = "CommentsByQuery")]
    public async Task<IActionResult> GetCommentsByQuery([FromRoute] string videoId, [FromQuery] QueryForCommentsPage queryForCommentsPage)
    {
        if (!ModelState.IsValid)
        {
            throw new ModelValidationException(ModelState);
        }
        
        var commentsResponse = 
            await _commentService.GetCommentsPageByQueryAsync(videoId, queryForCommentsPage);

        return Ok(commentsResponse);
    }
}
