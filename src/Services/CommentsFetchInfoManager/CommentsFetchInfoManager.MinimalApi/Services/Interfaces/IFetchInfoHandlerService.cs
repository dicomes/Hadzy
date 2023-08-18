using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IFetchInfoHandlerService
{
    APIResponse<T> CreateErrorResponse<T>(string errorMessage);
    Task<IResult> GetFetchStatusByIdAsync(string videoId);
    Task<IResult> PostNewFetchInfo(FetchInfoDto fetchInfoDto);
}