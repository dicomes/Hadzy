using CommentsFetchStatus.MinimalApi.Models;

namespace CommentsFetchStatus.MinimalApi.Services.Interfaces;

public interface IFetchStatusService
{
    Task<IResult> GetStatusByIdAsync(string videoId);
}