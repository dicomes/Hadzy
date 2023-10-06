using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Builders;

public class FetchInfoChangedEventBuilder
{
    private FetchInfoChangedEvent _event;

    public FetchInfoChangedEventBuilder(string videoId)
    {
        _event = new FetchInfoChangedEvent(videoId);
    }
    
    public FetchInfoChangedEventBuilder WithPageToken(string? pageToken)
    {
        _event.PageToken = pageToken;
        return this;
    }
    
    public FetchInfoChangedEventBuilder WithCommentIds(List<string>? commentIds)
    {
        if (commentIds != null)
        {
            _event.CommentIds = commentIds;
        }
        return this;
    }

    public FetchInfoChangedEventBuilder WithCommentsFetchedCount(ulong count)
    {
        _event.CommentsCount = count;
        return this;
    }

    public FetchInfoChangedEventBuilder WithReplyCount(uint count)
    {
        _event.ReplyCount = count;
        return this;
    }
    
    public FetchInfoChangedEventBuilder WithStatus(string? status)
    {
        _event.Status = status;
        return this;
    }

    public FetchInfoChangedEventBuilder WithCompletedTillFirstComment(bool completed)
    {
        _event.CompletedTillFirstComment = completed;
        return this;
    }

    public FetchInfoChangedEvent Build()
    {
        return _event;
    }
}

