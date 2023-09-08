using CommentsManager.Api.Enums;

namespace CommentsManager.Api.RequestParameters;

public class CommentsParameters : PageParameters
{
    public OrderBy OrderBy { get; set; } = OrderBy.PublishedDate;
    public Direction Direction { get; set; } = Direction.Ascending;
    public string? SearchTerm { get; set; }
    public string? Author { get; set; }
}