using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

namespace CommentsFetchInfoManager.MinimalApi.Services;

public class ErrorResponseService<T> : IErrorResponseService
{
    public ErrorResponseService()
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };
    }

    public APIResponse<T> CreateError<T>(List<string> errorMessages)
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };

        response.ErrorMessages = errorMessages;

        return response;
    }
    public APIResponse<T> CreateError<T>(string errorMessage)
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };

        response.ErrorMessages.Add(errorMessage);

        return response;
    }
}