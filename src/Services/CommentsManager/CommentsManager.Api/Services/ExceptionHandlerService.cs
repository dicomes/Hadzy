using System.Net;
using CommentsManager.Api.Contracts.Exceptions;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CommentsManager.Api.Services;

/// <summary>
/// Handles exceptions by converting them into appropriate HTTP responses.
/// </summary>
public class ExceptionHandlerService : IExceptionHandlerService
{
    private readonly ILogger<ExceptionHandlerService> _logger;
    public ExceptionHandlerService(
        ILogger<ExceptionHandlerService> logger)
    {
        _logger = logger;
    }

    public IActionResult HandleException(Exception ex)
    {
        // Handle model validation exceptions.
        if (ex is ModelValidationException modelValidationException)
        {
            // Extract validation error messages from the model state.
            var validationErrors = modelValidationException.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();

            var allErrors = new List<string> { modelValidationException.Message };
            allErrors.AddRange(validationErrors);

            // Return a BadRequest with the collected error messages.
            return new BadRequestObjectResult(new ApiResponse<PagedList<CommentResponse>>
            {
                ErrorMessages = allErrors
            });
        }

        if (ex is CommentNotFoundException commentNotFoundException)
        {
            var message = new List<string> { commentNotFoundException.Message };

            return new NotFoundObjectResult(new ApiResponse<CommentResponse>
            {
                ErrorMessages = message
            });
        }

        if (ex is ArgumentException argumentException)
        {
            var message = new List<string> { argumentException.Message };
            return new BadRequestObjectResult(new ApiResponse<PagedList<CommentResponse>>
            {
                ErrorMessages = message
            });
        }
        
        _logger.LogError(ex, "{Source}: Raised a {ExceptionType}. Exception Message: {ExceptionMessage}.", 
            GetType().Name, ex.GetType().Name, ex.Message);
            
        return new ObjectResult("An error occurred while processing your request.") 
        {
            StatusCode = (int)HttpStatusCode.InternalServerError 
        };
    }

}