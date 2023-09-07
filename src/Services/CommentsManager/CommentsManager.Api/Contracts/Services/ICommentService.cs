using System.Linq.Expressions;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Contracts.Services;

public interface ICommentService
{
    Task<CommentsPageResponse> GetCommentsPageByQueryAsync(string videoId, QueryForCommentsPage queryForCommentsPage);
}