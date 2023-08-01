using System.Net;
using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents
{
    public interface IFetcherErrorEvent
    {
        string Message { get; }
        ErrorType ErrorType { get; }
        string VideoId { get; }
    }
}