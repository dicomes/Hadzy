using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class FetchInfoChangedEvent : IFetchInfoChangedEvent
{
    public FetchInfoChangedEvent()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
    public string VideoId { get; set; }
    public string PageToken { get; set; }
    public int CommentsCount { get; set; }
    public List<string> CommentIds { get; set; }
    public int ReplyCount { get; set; }
    public string Status { get; set; }

    public override string ToString() =>
        $"CommentsFetchedStatusEvent - Guid: {Id}. VideoId: {VideoId}, NextPageToken: {PageToken}, " +
        $"CommentsCount: {CommentsCount}, ReplyCount: {ReplyCount}, Status: {Status}";
}