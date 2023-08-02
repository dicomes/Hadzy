using System.Net;
using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.Exceptions;

public class CommentsServiceException : Exception
{
    public string VideoId { get; }
    public ErrorCategory ErrorCategory { get; }

    public CommentsServiceException(string videoId, string message, ErrorCategory errorCategory)
        : base(message)
    {
        VideoId = videoId;
        ErrorCategory = errorCategory;
    }
}