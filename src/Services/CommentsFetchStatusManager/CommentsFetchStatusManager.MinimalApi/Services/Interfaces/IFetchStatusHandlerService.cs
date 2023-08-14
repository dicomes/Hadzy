using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;

namespace CommentsFetchStatus.MinimalApi.Services.Interfaces;

public interface IFetchStatusHandlerService
{
    APIResponse<T> CreateErrorResponse<T>(string errorMessage);
    Task<IResult> GetFetchStatusByIdAsync(string videoId);
    Task<IResult> PostNewFetchInfo(FetchInfoDto fetchInfoDto);


}