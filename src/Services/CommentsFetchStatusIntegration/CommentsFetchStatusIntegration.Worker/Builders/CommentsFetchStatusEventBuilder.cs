using CommentsFetchStatusIntegration.Worker.IntegrationEvents;

namespace CommentsFetchStatusIntegration.Worker.Builders;

public class CommentsFetchStatusEventBuilder
{
    private Guid _id;
    private string _videoId;
    private string _pageToken;
    private int _commentsFetchedCount;
    private int _replyCount;
    private bool _isFetching;

    public CommentsFetchStatusEventBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithVideoId(string videoId)
    {
        _videoId = videoId;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithPageToken(string pageToken)
    {
        _pageToken = pageToken;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithCommentsFetchedCount(int count)
    {
        _commentsFetchedCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithReplyCount(int count)
    {
        _replyCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithIsFetching(bool isFetching)
    {
        _isFetching = isFetching;
        return this;
    }

    public CommentsFetchStatusEvent Build()
    {
        return new CommentsFetchStatusEvent
        {
            Id = _id,
            VideoId = _videoId,
            PageToken = _pageToken,
            CommentsFetchedCount = _commentsFetchedCount,
            ReplyCount = _replyCount,
            IsFetching = _isFetching
        };
    }
}
