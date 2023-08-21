using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IFetchStatusHandler
{
    bool CanHandle(string? status);
    Task<IResult> HandleAsync(FetchInfoDto fetchInfoDto, VideoFetchInfo oldVideoFetchInfo);
}