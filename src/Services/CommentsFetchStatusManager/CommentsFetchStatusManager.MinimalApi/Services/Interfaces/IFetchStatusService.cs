using CommentsFetchStatus.MinimalApi.Models;

namespace CommentsFetchStatus.MinimalApi.Services.Interfaces;

public interface IFetchStatusService
{
    APIResponse<T> CreateErrorResponse<T>(string errorMessage);
    Task<IResult> GetStatus(string videoId);
}