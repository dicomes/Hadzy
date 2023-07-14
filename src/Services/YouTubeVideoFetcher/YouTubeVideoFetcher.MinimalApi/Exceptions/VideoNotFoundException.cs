namespace YouTubeVideoFetcher.MinimalApi.Exceptions;

public class VideoNotFoundException : Exception
{
    public VideoNotFoundException(string message)
        : base(message) { }
}