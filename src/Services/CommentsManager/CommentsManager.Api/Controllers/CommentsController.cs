using System.Net;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using CommentsManager.Api.RequestParameters;
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

    [ProducesResponseType(typeof(ApiResponse<PagedList<CommentResponse>>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedList<CommentResponse>>), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PagedList<CommentResponse>>), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [Produces("application/json")]
    [HttpGet(Name = "CommentsByQuery")]
    public async Task<IActionResult> GetCommentsByQuery([FromRoute] string videoId, [FromQuery] CommentsParameters parameters)
    {
        if (!ModelState.IsValid)
        {
            throw new ModelValidationException(ModelState);
        }
        
        var pagedList = await _commentService.GetCommentsPageByQueryAsync(videoId, parameters);
        
        Console.WriteLine(pagedList.PageInfo.ToString());
        Console.WriteLine(pagedList.Items.ToString());

        var apiResponse = new ApiResponse<PagedList<CommentResponse>>()
        {
            Result = pagedList
        };
        
        return Ok(apiResponse);
    }
}
