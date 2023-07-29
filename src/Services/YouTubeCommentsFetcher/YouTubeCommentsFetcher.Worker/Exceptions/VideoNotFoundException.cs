namespace YouTubeCommentsFetcher.Worker.Exceptions;

public class VideoNotFoundException : Exception
{
    public VideoNotFoundException(string message) 
        : base(message) { }
}