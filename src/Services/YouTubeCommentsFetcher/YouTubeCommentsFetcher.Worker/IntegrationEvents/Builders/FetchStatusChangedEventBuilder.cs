using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class FetchStatusChangedEventBuilder
{
    private FetchInfoChangedEvent _changedEvent;

    public FetchStatusChangedEventBuilder()
    {
        _changedEvent = new FetchInfoChangedEvent();
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
        _changedEvent.CommentsCount = count;
        return this;
    }

    public FetchStatusChangedEventBuilder WithReplyCount(int count)
    {
        _changedEvent.ReplyCount = count;
        return this;
    }
    
    public FetchStatusChangedEventBuilder WithStatus(string status)
    {
        _changedEvent.Status = status;
        return this;
    }

    public FetchInfoChangedEvent Build()
    {
        return _changedEvent;
    }
}

