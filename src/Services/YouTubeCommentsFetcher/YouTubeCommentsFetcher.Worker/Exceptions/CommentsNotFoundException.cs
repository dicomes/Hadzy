namespace YouTubeCommentsFetcher.Worker.Exceptions;

public class CommentsNotFoundException : Exception
{
    public CommentsNotFoundException(string message) 
        : base(message) { }
}