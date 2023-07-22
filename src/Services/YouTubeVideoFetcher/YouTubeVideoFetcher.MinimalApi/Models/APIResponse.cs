namespace YouTubeVideoFetcher.MinimalApi.Models;

public class APIResponse<T>
{
    public List<string> ErrorMessages { get; set; }
    public T? Result { get; set; }

    public APIResponse()
    {
        ErrorMessages = new List<string>();
    }
}
