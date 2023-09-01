using AutoMapper;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;
using CommentsManager.Api.Repositories;
using CommentsManager.Api.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult<List<GetCommentResponse>>> Get(string id)
    {
        var comments = await _commentService.GetAllCommentsByVideoIdAsync(id);
        var result = _mapper.Map<List<GetCommentResponse>>(comments);
        return Ok(result);
    }
}
