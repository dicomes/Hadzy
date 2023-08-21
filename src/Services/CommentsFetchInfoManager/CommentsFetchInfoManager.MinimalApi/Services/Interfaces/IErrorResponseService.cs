using CommentsFetchInfoManager.MinimalApi.Models;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IErrorResponseService
{
    APIResponse<T> CreateErrorResponse<T>(List<string> errorMessages);
}