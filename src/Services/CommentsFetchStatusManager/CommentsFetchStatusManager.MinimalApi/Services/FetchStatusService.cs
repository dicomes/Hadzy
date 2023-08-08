using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Services.Interfaces;

namespace CommentsFetchStatus.MinimalApi.Services;

public class FetchStatusService : IFetchStatusService
{
    public APIResponse<T> CreateErrorResponse<T>(string errorMessage)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> GetStatus(string videoId)
    {
        throw new NotImplementedException();
    }
}