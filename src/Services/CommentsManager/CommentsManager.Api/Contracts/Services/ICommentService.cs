using CommentsManager.Api.DTO;
using CommentsManager.Api.RequestParameters;

namespace CommentsManager.Api.Contracts.Services;

public interface ICommentService
{
    Task<PagedList<CommentResponse>> GetCommentsPageByQueryAsync(string videoId, CommentsParameters parameters);
}