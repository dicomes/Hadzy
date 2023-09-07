namespace CommentsManager.Api.DTO;

public record PageInfo
{
    public int TotalElements { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}