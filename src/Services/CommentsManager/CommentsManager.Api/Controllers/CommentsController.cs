using System.Net;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.RequestParameters;
using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Controllers;

[ApiController]
[Route("comments-manager/api/v1/video/{videoId}/")]
public class CommentsController : ControllerBase
{
    private readonly ILogger<CommentsController> _logger;
    private readonly ICommentService _commentService;
    private readonly IVideoService _videoService;

    public CommentsController(
        ILogger<CommentsController> logger,
        ICommentService commentService,
        IVideoService videoService)
    {
        _logger = logger;
        _commentService = commentService;
        _videoService = videoService;
    }

    [ProducesResponseType(typeof(ApiResponse<PagedList<CommentResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedList<CommentResponse>>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PagedList<CommentResponse>>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Produces("application/json")]
    [HttpGet($"comments", Name = "CommentsByQuery")]
    public async Task<IActionResult> GetCommentsByQuery([FromRoute] string videoId, [FromQuery] CommentsParameters parameters)
    {
        if (!ModelState.IsValid)
        {
            throw new ModelValidationException(ModelState);
        }
        
        var pagedList = await _commentService.GetCommentsPageByQueryAsync(videoId, parameters);
        
        var apiResponse = new ApiResponse<PagedList<CommentResponse>>()
        {
            Result = pagedList
        };
        
        return Ok(apiResponse);
    }
    
    [ProducesResponseType(typeof(ApiResponse<CommentResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse<CommentResponse>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiResponse<CommentResponse>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Produces("application/json")]
    [HttpGet($"firstcomment", Name = "FirstCommentByVideoId")]
    public async Task<IActionResult> GetFirstCommentByVideoId([FromRoute] string videoId)
    {
        if (!ModelState.IsValid)
        {
            throw new ModelValidationException(ModelState);
        }

        var video = await _videoService.GetByIdAsync(videoId, false);

        if (video == null)
        {
            throw new CommentNotFoundException(videoId);
        }
        
        if (string.IsNullOrEmpty(video.FirstComment))
        {
            throw new CommentNotFoundException(videoId);
        }
        
        CommentResponse commentResponse = new CommentResponse();

        commentResponse = await _commentService.GetCommentByIdAsync(video.FirstComment);
        
        var apiResponse = new ApiResponse<CommentResponse>()
        {
            Result = commentResponse
        };
        
        return Ok(apiResponse);
    }
}
