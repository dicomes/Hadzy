namespace YouTubeVideoFetcher.MinimalApi.Exceptions;

public class VideoBadRequestException : Exception
{
    public VideoBadRequestException(string message) 
        : base(message) { }
}
