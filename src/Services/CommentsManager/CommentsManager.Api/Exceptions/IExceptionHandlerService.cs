using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Exceptions;

public interface IExceptionHandlerService
{
    public IActionResult HandleException(Exception exception);
}