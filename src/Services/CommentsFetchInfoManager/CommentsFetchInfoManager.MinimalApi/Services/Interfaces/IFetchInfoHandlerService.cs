using CommentsFetchInfoManager.MinimalApi.Models.DTO;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IFetchInfoHandlerService
{
    Task<IResult> HandleAsync(FetchInfoDto? fetchInfoDto);
}