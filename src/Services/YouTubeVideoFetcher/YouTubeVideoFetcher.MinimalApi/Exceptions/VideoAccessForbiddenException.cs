namespace YouTubeVideoFetcher.MinimalApi.Exceptions;

public class VideoAccessForbiddenException : Exception
{
    public VideoAccessForbiddenException(string message) 
        : base(message) { }
}