using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class FetchInfoChangedEvent : IFetchInfoChangedEvent
{
    public FetchInfoChangedEvent(string videoId)
    {
        Id = Guid.NewGuid();
        VideoId = videoId;
    }

    public Guid Id { get; }
    public string VideoId { get; }
    public string? PageToken { get; set; }
    public ulong CommentsCount { get; set; }
    public List<string>? CommentIds { get; set; }
    public uint ReplyCount { get; set; }
    public string? Status { get; set; }
    public bool CompletedTillFirstComment { get; set; }

    public override string ToString() =>
        $"CommentsFetchedStatusEvent - Guid: {Id}. VideoId: {VideoId}, NextPageToken: {PageToken}, " +
        $"CommentsCount: {CommentsCount}, ReplyCount: {ReplyCount}, Status: {Status}";
}