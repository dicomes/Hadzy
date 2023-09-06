using System.Net;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Services;
using CommentsManager.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Exceptions;


public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly IErrorResponseService _errorResponseService;

    public ExceptionHandlerService(IErrorResponseService errorResponseService)
    {
        _errorResponseService = errorResponseService;
    }

    public IActionResult HandleException(Exception exception)
    {
        if (exception is ModelValidationException modelValidationException)
        {
            var validationErrors = modelValidationException.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();

            var allErrors = new List<string> { modelValidationException.Message };
            allErrors.AddRange(validationErrors);

            return new BadRequestObjectResult(new ApiResponse<object>
            {
                ErrorMessages = allErrors,
            });
        }
    
        return new ObjectResult("An error occurred while processing your request.") 
        { 
            StatusCode = (int)HttpStatusCode.InternalServerError 
        };
    }

}