namespace YouTubeCommentsFetcher.Worker.Exceptions;

public class CommentsAccessForbiddenException : Exception
{
    public CommentsAccessForbiddenException(string message) 
        : base(message) { }
}