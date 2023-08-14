using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Builders;

public class CommentsFetchStatusEventBuilder
{
    private readonly FetchInfoChangedEvent _changedEvent;

    public CommentsFetchStatusEventBuilder()
    {
        _changedEvent = new FetchInfoChangedEvent();
    }

    public CommentsFetchStatusEventBuilder WithId(Guid id)
    {
        _changedEvent.Id = id;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithVideoId(string videoId)
    {
        _changedEvent.VideoId = videoId;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithPageToken(string pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _changedEvent.PageToken = pageToken;
        }
        
        return this;
    }

    public CommentsFetchStatusEventBuilder WithCommentsFetchedCount(int count)
    {
        _changedEvent.CommentsCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithReplyCount(int count)
    {
        _changedEvent.ReplyCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithStatus(string status)
    {
        _changedEvent.Status = status;
        return this;
    }

    public FetchInfoChangedEvent Build()
    {
        return _changedEvent;
    }
    
    public FetchInfoChangedEvent BuildFromEvent(IFetchInfoChangedEvent message)
    {
        return new CommentsFetchStatusEventBuilder()
            .WithId(message.Id)
            .WithVideoId(message.VideoId)
            .WithPageToken(message.PageToken)
            .WithCommentsFetchedCount(message.CommentsCount)
            .WithReplyCount(message.ReplyCount)
            .WithStatus(message.Status)
            .Build();
    }
}
