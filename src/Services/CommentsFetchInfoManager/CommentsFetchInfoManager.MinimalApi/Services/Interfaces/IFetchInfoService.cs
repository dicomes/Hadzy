using CommentsFetchInfoManager.MinimalApi.Models.DTO;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IFetchInfoService
{
    Task<IResult> PostNewFetchInfo(FetchInfoDto? fetchInfoDto);
}