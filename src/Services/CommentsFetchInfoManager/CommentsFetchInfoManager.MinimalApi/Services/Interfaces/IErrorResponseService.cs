using CommentsFetchInfoManager.MinimalApi.Models;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IErrorResponseService
{
    APIResponse<T> CreateError<T>(List<string> errorMessages);
    APIResponse<T> CreateError<T>(string errorMessage);
}