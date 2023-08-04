using System.Net;
using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents
{
    public interface IInternalFetcherErrorEvent
    {
        string Message { get; }
        ErrorCategory ErrorCategory { get; }
        string VideoId { get; }
    }
}