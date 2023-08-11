using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsThreadMapper
{
    FetchCompletedEvent MapToFetchCompletedEvent(string videoId, CommentThreadListResponse response);
}