namespace CommentsManager.Api.DTO;

public record CommentsPageResponse()
{
    public IEnumerable<CommentForResponse> Comments { get; set; }
    public PageInfo PageInfo { get; set; } = new PageInfo();
}
