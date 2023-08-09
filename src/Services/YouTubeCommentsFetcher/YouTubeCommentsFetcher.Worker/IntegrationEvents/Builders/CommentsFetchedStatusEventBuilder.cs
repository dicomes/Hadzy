namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class CommentsFetchedStatusEventBuilder
{
    private CommentsFetchStatusEvent _event;

    public CommentsFetchedStatusEventBuilder()
    {
        _event = new CommentsFetchStatusEvent();
        _event.Id = Guid.NewGuid();
    }

    public CommentsFetchedStatusEventBuilder WithVideoId(string videoId)
    {
        _event.VideoId = videoId;
        return this;
    }

    public CommentsFetchedStatusEventBuilder WithPageToken(string pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _event.PageToken = pageToken;
        }
        return this;
    }

    public CommentsFetchedStatusEventBuilder WithCommentsFetchedCount(int count)
    {
        _event.CommentsFetchedCount = count;
        return this;
    }

    public CommentsFetchedStatusEventBuilder WithReplyCount(int count)
    {
        _event.ReplyCount = count;
        return this;
    }
    
    public CommentsFetchedStatusEventBuilder WithIsFetching(bool isFetching)
    {
        _event.IsFetching = isFetching;
        return this;
    }

    public CommentsFetchStatusEvent Build()
    {
        return _event;
    }
}

