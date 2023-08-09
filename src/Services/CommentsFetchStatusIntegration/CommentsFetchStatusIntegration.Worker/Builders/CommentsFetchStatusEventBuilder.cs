using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Builders;

public class CommentsFetchStatusEventBuilder
{
    private readonly CommentsFetchStatusEvent _event;

    public CommentsFetchStatusEventBuilder()
    {
        _event = new CommentsFetchStatusEvent();
    }

    public CommentsFetchStatusEventBuilder WithId(Guid id)
    {
        _event.Id = id;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithVideoId(string videoId)
    {
        _event.VideoId = videoId;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithPageToken(string pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _event.PageToken = pageToken;
        }
        
        return this;
    }

    public CommentsFetchStatusEventBuilder WithCommentsFetchedCount(int count)
    {
        _event.CommentsFetchedCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithReplyCount(int count)
    {
        _event.ReplyCount = count;
        return this;
    }

    public CommentsFetchStatusEventBuilder WithIsFetching(bool isFetching)
    {
        _event.IsFetching = isFetching;
        return this;
    }

    public CommentsFetchStatusEvent Build()
    {
        return _event;
    }
    
    public CommentsFetchStatusEvent BuildFromMessage(ICommentsFetchStatusEvent message)
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
