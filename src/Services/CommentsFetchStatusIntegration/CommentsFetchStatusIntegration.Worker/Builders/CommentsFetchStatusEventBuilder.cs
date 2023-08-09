using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Builders;

public class CommentsFetchStatusEventBuilder
{
    private readonly FetchStatusChangedEvent _changedEvent;

    public CommentsFetchStatusEventBuilder()
    {
        _changedEvent = new FetchStatusChangedEvent();
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
        _changedEvent.CommentsFetchedCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithReplyCount(int count)
    {
        _changedEvent.ReplyCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithIsFetching(bool isFetching)
    {
        _changedEvent.IsFetching = isFetching;
        return this;
    }

    public FetchStatusChangedEvent Build()
    {
        return _changedEvent;
    }
    
    public FetchStatusChangedEvent BuildFromEvent(IFetchStatusChangedEvent message)
    {
        return new CommentsFetchStatusEventBuilder()
            .WithId(message.Id)
            .WithVideoId(message.VideoId)
            .WithPageToken(message.PageToken)
            .WithCommentsFetchedCount(message.CommentsFetchedCount)
            .WithReplyCount(message.ReplyCount)
            .WithIsFetching(message.IsFetching)
            .Build();
    }
}
