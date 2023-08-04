using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface ICommentsFetchedStatusEventFactory
{
    CommentsFetchedStatusEvent Create(string videoId, string pageToken, int commentsFetchedCount, int replyCount);
}