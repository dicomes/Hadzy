using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class CommentsFetchedEventBuilder
{
    private readonly FetchCompletedEvent _event;

    public CommentsFetchedEventBuilder()
    {
        _event = new FetchCompletedEvent
        {
            Id = Guid.NewGuid()
        };
    }

    public CommentsFetchedEventBuilder WithVideoId(string videoId)
    {
        _event.VideoId = videoId;
        return this;
    }

    public CommentsFetchedEventBuilder WithPageToken(string pageToken)
    {
        _event.PageToken = pageToken;
        return this;
    }

    public CommentsFetchedEventBuilder WithCommentsFetchedCount(int count)
    {
        _event.CommentsFetchedCount = count;
        return this;
    }

    public CommentsFetchedEventBuilder WithReplyCount(int count)
    {
        _event.ReplyCount = count;
        return this;
    }

    public CommentsFetchedEventBuilder WithYouTubeCommentsList(List<YouTubeCommentDto> commentsList)
    {
        _event.YouTubeCommentsList = commentsList.Cast<IYouTubeCommentDto>().ToList();
        return this;
    }

    public FetchCompletedEvent Build()
    {
        return _event;
    }
}