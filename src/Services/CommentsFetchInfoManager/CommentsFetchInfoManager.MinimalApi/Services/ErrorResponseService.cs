using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class ErrorResponseService : IErrorResponseService
{
    public APIResponse<T> CreateErrorResponse<T>(List<string> errorMessages)
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };

        response.ErrorMessages = errorMessages;

        return response;
    }
}