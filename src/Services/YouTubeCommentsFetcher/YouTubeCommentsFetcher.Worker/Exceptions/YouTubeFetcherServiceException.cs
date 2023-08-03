using System.Net;
using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.Exceptions;

public class YouTubeFetcherServiceException : Exception
{
    public string VideoId { get; }
    public ErrorCategory ErrorCategory { get; }

    public YouTubeFetcherServiceException(string videoId, string message, ErrorCategory errorCategory)
        : base(message)
    {
        VideoId = videoId;
        ErrorCategory = errorCategory;
    }
}