using CommentsFetchInfoIntegration.Worker.IntegrationEvents;
using IntegrationEventsContracts;

namespace CommentsFetchInfoIntegration.Worker.Builders;

public class CommentsFetchInfoEventBuilder
{
    private readonly FetchInfoChangedEvent _changedEvent;

    public CommentsFetchInfoEventBuilder()
    {
        _changedEvent = new FetchInfoChangedEvent();
    }

    public CommentsFetchInfoEventBuilder WithId(Guid id)
    {
        _changedEvent.Id = id;
        return this;
    }

    public CommentsFetchInfoEventBuilder WithVideoId(string? videoId)
    {
        _changedEvent.VideoId = videoId;
        return this;
    }

    public CommentsFetchInfoEventBuilder WithPageToken(string? pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _changedEvent.PageToken = pageToken;
        }
        
        return this;
    }
    
    public CommentsFetchInfoEventBuilder WithCommentIds(List<string>? commentIds)
    {
        _changedEvent.CommentIds = commentIds;
        return this;
    }
    
    public CommentsFetchInfoEventBuilder WithCommentsFetchedCount(int count)
    {
        _changedEvent.CommentsCount = count;
        return this;
    }

    public CommentsFetchInfoEventBuilder WithReplyCount(int count)
    {
        _changedEvent.ReplyCount = count;
        return this;
    }

    public CommentsFetchInfoEventBuilder WithStatus(string? status)
    {
        _changedEvent.Status = status;
        return this;
    }
    
    public CommentsFetchInfoEventBuilder CompletedTillFirstComment(bool completed)
    {
        _changedEvent.CompletedTillFirstComment = completed;
        return this;
    }

    public FetchInfoChangedEvent Build()
    {
        return _changedEvent;
    }
    
    public FetchInfoChangedEvent BuildFromEvent(IFetchInfoChangedEvent message)
    {
        return new CommentsFetchInfoEventBuilder()
            .WithId(message.Id)
            .WithVideoId(message.VideoId)
            .WithPageToken(message.PageToken)
            .WithCommentIds(message.CommentIds)
            .WithCommentsFetchedCount(message.CommentsCount)
            .WithReplyCount(message.ReplyCount)
            .WithStatus(message.Status)
            .CompletedTillFirstComment(message.CompletedTillFirstComment)
            .Build();
    }
}
