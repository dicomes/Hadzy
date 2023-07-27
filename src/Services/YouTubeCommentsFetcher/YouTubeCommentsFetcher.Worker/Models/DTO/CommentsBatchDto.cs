namespace YouTubeCommentsFetcher.Worker.Models.DTO;

public class CommentsBatchDto
{
    public string VideoId { get; set; }
    public List<YouTubeComment> YouTubeCommentsList { get; set; }
}