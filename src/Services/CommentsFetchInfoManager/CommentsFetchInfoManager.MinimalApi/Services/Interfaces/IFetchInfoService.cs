using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IFetchInfoService
{
    Task<IResult> GetFetchInfoByIdAsync(string? videoId);
    Task<IResult> CreateNewFetchInfoAsync(FetchInfoDto fetchInfoDto);
}