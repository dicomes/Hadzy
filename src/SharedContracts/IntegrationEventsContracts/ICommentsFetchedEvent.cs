namespace IntegrationEventsContracts;

public interface ICommentsFetchedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}