using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.IntegrationEvents;

public class VideoCommentsStatusEvent : IVideoCommentsStatusEvent
{
    public Guid Id { get; set; }
    public string VideoId { get; set; }
    public int TotalCommentsFetched { get; set; }
    public bool IsFetching { get; set; }
}