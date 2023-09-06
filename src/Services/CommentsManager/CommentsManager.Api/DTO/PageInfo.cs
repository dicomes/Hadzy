namespace CommentsManager.Api.DTO;

public record PageInfo
{
    public uint TotalElements { get; set; }
    public uint PageNumber { get; set; }
    public uint PageSize { get; set; }
    public uint TotalPages { get; set; }
}