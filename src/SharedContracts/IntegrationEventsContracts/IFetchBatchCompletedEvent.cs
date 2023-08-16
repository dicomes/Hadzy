namespace IntegrationEventsContracts;

public interface IFetchCompletedEvent
{
    Guid Id { get; }
    string VideoId { get; }
    string NextPageToken { get; }
    List<IYouTubeCommentDto> YouTubeCommentsList { get; }
}