using System.ComponentModel.DataAnnotations;

namespace CommentsManager.Api.DTO;

public record QueryForCommentsPage
{
    [Required(ErrorMessage = "Page is mandatory.")]
    public int? Page { get; set; }
    [Required(ErrorMessage = "Size is mandatory.")]
    public int? Size { get; set; }
    public string? SortBy { get; set; }
    public string? Direction { get; set; }
    public string? SearchTerm { get; set; }
    public string? Author { get; set; }
}