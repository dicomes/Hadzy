namespace IntegrationEventsContracts;

public interface IFetchCompletedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string PageToken { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}