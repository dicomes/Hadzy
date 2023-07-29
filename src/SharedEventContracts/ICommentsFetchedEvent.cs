namespace SharedEventContracts;

public interface ICommentsFetchedEvent
{
    string VideoId { get; }
    public string PageToken { get; set; } 
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}