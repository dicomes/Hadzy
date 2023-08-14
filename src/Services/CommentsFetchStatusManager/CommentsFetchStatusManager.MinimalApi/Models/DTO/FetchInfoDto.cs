using System.ComponentModel.DataAnnotations;

namespace CommentsFetchStatus.MinimalApi.Models.DTO;

public class FetchInfoDto
{
    public string VideoId { get; set; }
    public int CommentsCount { get; set; }
    public string Status { get; set; }
}