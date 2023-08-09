using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class FetchCompletedEvent : IFetchCompletedEvent
{
    public Guid Id { get; set; }
    public string VideoId { get; set; }
    public string NextPageToken { get; set; } 
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; set; }
    public List<IYouTubeCommentDto> YouTubeCommentsList { get; set; }

    public override string ToString() =>
        $"FetchCompletedEvent - Guid {Id} .VideoId: {VideoId}, PageToken: {NextPageToken}, " +
        $"CommentsFetchedCount: {CommentsFetchedCount}, ReplyCount: {ReplyCount}";
}