namespace SharedEventContracts;

public interface ICommentsFetchedEvent
{
    string VideoId { get; }
    public string PageToken { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}