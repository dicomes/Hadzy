using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface ICommentsThreadMapper
{
    CommentThreadListCompletedEvent ToBatchCompletedEvent(string? videoId, CommentThreadListResponse response);
}