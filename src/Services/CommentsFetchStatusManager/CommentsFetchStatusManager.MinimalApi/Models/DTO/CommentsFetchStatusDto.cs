using IntegrationEventsContracts;

namespace CommentsFetchStatus.MinimalApi.Models.DTO;

public class CommentsFetchStatusDto
{
    public string VideoId { get; set; }
    public string PageToken { get; set; }
    public int CommentsFetchedCount { get; set; }
    public int ReplyCount { get; set; }
}