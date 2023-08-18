using IntegrationEventsContracts;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class FetchInfoChangedEventBuilder
{
    private FetchInfoChangedEvent _event;

    public FetchInfoChangedEventBuilder()
    {
        _event = new FetchInfoChangedEvent();
    }

    public FetchInfoChangedEventBuilder WithVideoId(string videoId)
    {
        _event.VideoId = videoId;
        return this;
    }

    public FetchInfoChangedEventBuilder WithPageToken(string pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _event.PageToken = pageToken;
        }
        return this;
    }
    
    public FetchInfoChangedEventBuilder WithCommentIds(List<string> commentIds)
    {
        _event.CommentIds = commentIds;
        return this;
    }

    public FetchInfoChangedEventBuilder WithCommentsFetchedCount(int count)
    {
        _event.CommentsCount = count;
        return this;
    }

    public FetchInfoChangedEventBuilder WithReplyCount(int count)
    {
        _event.ReplyCount = count;
        return this;
    }
    
    public FetchInfoChangedEventBuilder WithStatus(string status)
    {
        _event.Status = status;
        return this;
    }

    public FetchInfoChangedEvent Build()
    {
        return _event;
    }
}

