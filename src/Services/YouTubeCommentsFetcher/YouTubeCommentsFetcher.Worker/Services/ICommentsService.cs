using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Services;

public interface ICommentsService
{
    Task<CommentsBatchDto> GetCommentsByVideoIdAsync(string videoId, int maxResults);
}