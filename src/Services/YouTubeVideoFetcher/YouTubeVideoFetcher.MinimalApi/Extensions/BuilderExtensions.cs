using Serilog;
using YouTubeVideoFetcher.MinimalApi.Configurations;

namespace YouTubeVideoFetcher.MinimalApi.Extensions
{
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

        public static WebApplicationBuilder ConfigureUserSecretsInDevelopment(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddUserSecrets<Program>();
            }
            return builder;
        }
    }
}