using CommentsManager.Api.DTO;
using CommentsManager.Api.Services.Interfaces;

namespace CommentsManager.Api.Services;

public class ErrorResponseService : IErrorResponseService
{
    public ApiResponse<T> CreateError<T>(List<string> errorMessages)
    {
        var response = new ApiResponse<T>
        {
            Result = default,
            ErrorMessages = errorMessages
        };

        return response;
    }

    public ApiResponse<T> CreateError<T>(string errorMessage)
    {
        return CreateError<T>(new List<string> { errorMessage });
    }
}