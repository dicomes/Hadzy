namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class CommentsFetchedStatusEventBuilder
{
    private CommentsFetchedStatusEvent _event;

    public CommentsFetchedStatusEventBuilder()
    {
        _event = new CommentsFetchedStatusEvent();
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

    public CommentsFetchedStatusEvent Build()
    {
        return _event;
    }
}

