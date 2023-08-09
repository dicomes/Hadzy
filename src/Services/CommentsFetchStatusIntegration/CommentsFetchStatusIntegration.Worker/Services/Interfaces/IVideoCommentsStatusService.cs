using CommentsFetchStatusIntegration.Worker.Models;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface IVideoCommentsStatusService
{
    Task<bool> VideoIdExistsAsync(string videoId);
    Task UpdateVideoCommentsStatusAsync(string videoId, int newTotalCommentsFetched, bool isFetching);
    Task UpdateVideoCommentsStatusAsync(VideoCommentsStatus videoCommentsStatus);
    Task<VideoCommentsStatus> GetVideoCommentsStatusByVideoIdAsync(string videoId);
    Task InsertVideoCommentsStatusAsync(VideoCommentsStatus videoCommentsStatus);
}