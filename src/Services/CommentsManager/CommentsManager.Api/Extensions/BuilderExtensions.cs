using CommentsManager.Api.Configurations;
using Serilog;

namespace CommentsManager.Api.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        var seqConfig = builder.Configuration.GetSection("Seq");
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig["Url"])
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
    
    public static void ConfigurePostgreSqlSettings(this WebApplicationBuilder builder) =>
        builder.Services.Configure<PostgreSqlConfig>(
            builder.Configuration.GetSection("PostgreSql"));
}