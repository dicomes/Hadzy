using System.ComponentModel.DataAnnotations;

namespace CommentsManager.Api.DTO;

public record GetComment
{
    [Required(ErrorMessage = "Page is mandatory.")]
    public uint? Page { get; set; }
    [Required(ErrorMessage = "Size is mandatory.")]
    public uint? Size { get; set; }
    public string? SortBy { get; set; }
    public string? Direction { get; set; }
    public string? SearchTerm { get; set; }
    public string? Author { get; set; }
}