namespace CommentsManager.Api.DTO;

public record GetCommentResponse()
{
    public IEnumerable<GetComment> Comments { get; set; }
    public PageInfo PageInfo { get; set; }
}
