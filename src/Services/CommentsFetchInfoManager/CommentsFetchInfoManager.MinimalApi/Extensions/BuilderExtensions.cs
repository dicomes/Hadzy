using CommentsFetchInfoManager.MinimalApi.Configurations;
using Serilog;

namespace CommentsFetchInfoManager.MinimalApi.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder UseSeqLogger(this WebApplicationBuilder builder, SeqConfig seqConfig)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Seq(seqConfig.Url)
            .WriteTo.Console()
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}