using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Contracts.Exceptions;

public interface IExceptionHandlerService
{
    public IActionResult HandleException(Exception exception);
}