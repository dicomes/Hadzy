using System.Net;
using CommentsManager.Api.Contracts.Exceptions;
using CommentsManager.Api.Contracts.Services;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Services;

/// <summary>
/// Handles exceptions by converting them into appropriate HTTP responses.
/// </summary>
public class ExceptionHandlerService : IExceptionHandlerService
{
    public IActionResult HandleException(Exception exception)
    {
        // Handle model validation exceptions.
        if (exception is ModelValidationException modelValidationException)
        {
            // Extract validation error messages from the model state.
            var validationErrors = modelValidationException.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToList();

            var allErrors = new List<string> { modelValidationException.Message };
            allErrors.AddRange(validationErrors);

            // Return a BadRequest with the collected error messages.
            return new BadRequestObjectResult(new ApiResponse<QueryForCommentsPage>
            {
                ErrorMessages = allErrors
            });
        }

        if (exception is CommentNotFoundException commentNotFoundException)
        {
            var message = new List<string> { commentNotFoundException.Message };

            return new NotFoundObjectResult(new ApiResponse<QueryForCommentsPage>
            {
                ErrorMessages = message
            });
        }
            
        // Default error response for unhandled exceptions.
        return new ObjectResult("An error occurred while processing your request.") 
        { 
            StatusCode = (int)HttpStatusCode.InternalServerError 
        };
    }

}