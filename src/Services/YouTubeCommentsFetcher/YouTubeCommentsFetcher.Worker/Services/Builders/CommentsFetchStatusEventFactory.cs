using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services.Builders;

public class CommentsFetchStatusEventFactory : ICommentsFetchedStatusEventFactory
{
    public CommentsFetchedStatusEvent Create(string videoId, string pageToken, int commentsFetchedCount, int replyCount)
    {
        return new CommentsFetchedStatusEvent
        {
            VideoId = videoId,
            PageToken = pageToken,
            CommentsFetchedCount = commentsFetchedCount,
            ReplyCount = replyCount
        };
    }
}
