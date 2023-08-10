namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class FetchStatusChangedEventBuilder
{
    private FetchStatusChangedEvent _changedEvent;

    public FetchStatusChangedEventBuilder()
    {
        _changedEvent = new FetchStatusChangedEvent();
        _changedEvent.Id = Guid.NewGuid();
    }

    public FetchStatusChangedEventBuilder WithVideoId(string videoId)
    {
        _changedEvent.VideoId = videoId;
        return this;
    }

    public FetchStatusChangedEventBuilder WithPageToken(string pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _changedEvent.PageToken = pageToken;
        }
        return this;
    }

    public FetchStatusChangedEventBuilder WithCommentsFetchedCount(int count)
    {
        _changedEvent.CommentsFetchedCount = count;
        return this;
    }

    public FetchStatusChangedEventBuilder WithReplyCount(int count)
    {
        _changedEvent.ReplyCount = count;
        return this;
    }
    
    public FetchStatusChangedEventBuilder WithIsFetching(bool isFetching)
    {
        _changedEvent.IsFetching = isFetching;
        return this;
    }

    public FetchStatusChangedEvent Build()
    {
        return _changedEvent;
    }
}

