using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents;

public class CommentsFetchedEvent : ICommentsFetchedEvent
{
    public string VideoId { get; set; }
    
    public string PageToken { get; set; } 
    public List<YouTubeComment> YouTubeCommentsList { get; set; }
}