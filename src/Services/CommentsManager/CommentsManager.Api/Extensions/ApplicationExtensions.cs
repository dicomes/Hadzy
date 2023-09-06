using CommentsManager.Api.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CommentsManager.Api.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication UseCustomExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature.Error;
                var handlerService = context.RequestServices.GetRequiredService<IExceptionHandlerService>();
                var result = handlerService.HandleException(exception);

                var actionContext = new ActionContext
                {
                    HttpContext = context
                };

                await result.ExecuteResultAsync(actionContext);
            });
        });

        return app;
    }

}