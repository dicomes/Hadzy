using CommentsManager.Api.DTO;

namespace CommentsManager.Api.Services.Interfaces;

public interface IErrorResponseService
{
    ApiResponse<T> CreateError<T>(List<string> errorMessages);
    ApiResponse<T> CreateError<T>(string errorMessage);
}