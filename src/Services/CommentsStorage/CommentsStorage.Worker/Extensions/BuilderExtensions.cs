using Microsoft.AspNetCore.Builder;
using Serilog;

namespace CommentsStorage.Worker.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(builder.Configuration["Seq:Url"])
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}