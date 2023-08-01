using System.Net;
using YouTubeCommentsFetcher.Worker.Enums;

namespace YouTubeCommentsFetcher.Worker.Exceptions;

public class CommentsServiceException : Exception
{
    public string VideoId { get; }
    public ErrorType ErrorType { get; }

    public CommentsServiceException(string videoId, string message, ErrorType errorType)
        : base(message)
    {
        VideoId = videoId;
        ErrorType = errorType;
    }
}