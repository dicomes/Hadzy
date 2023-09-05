using System.Linq.Expressions;
using AutoMapper;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;
using CommentsManager.Api.Repositories;
using CommentsManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CommentsManager.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> GetCommentsFiltered(
        [FromQuery] string videoId)
    {
        Expression<Func<Comment, bool>> filterExpression = comment => comment.VideoId == videoId;

        var comments = await _commentService.FindByConditionAsync(filterExpression);
            
        var commentResponses = _mapper.Map<List<GetCommentResponse>>(comments);

        return Ok(commentResponses);
    }
}
