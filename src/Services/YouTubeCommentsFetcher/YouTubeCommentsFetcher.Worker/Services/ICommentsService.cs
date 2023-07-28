using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Services;

public interface ICommentsService
{
    Task<CommentsBatchDto> GetCommentBatchByVideoIdAsync(FetchSettings fetchSettings);
}