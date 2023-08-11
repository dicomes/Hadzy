using IntegrationEventsContracts;

namespace CommentsFetchStatus.MinimalApi.Models.DTO;

public class CommentsFetchStatusDto
{
    public string VideoId { get; set; }
    public int CommentsCount { get; set; }
    public bool IsFetching { get; set; }
}