using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class FetchCompletedEventBuilder
{
    private readonly FetchCompletedEvent _event;

    public FetchCompletedEventBuilder()
    {
        _event = new FetchCompletedEvent
        {
            Id = Guid.NewGuid()
        };
    }

    public FetchCompletedEventBuilder WithVideoId(string videoId)
    {
        _event.VideoId = videoId;
        return this;
    }

    public FetchCompletedEventBuilder WithPageToken(string pageToken)
    {
        _event.NextPageToken = pageToken;
        return this;
    }

    public FetchCompletedEventBuilder WithCommentsFetchedCount(int count)
    {
        _event.CommentsFetchedCount = count;
        return this;
    }

    public FetchCompletedEventBuilder WithReplyCount(int count)
    {
        _event.ReplyCount = count;
        return this;
    }

    public FetchCompletedEventBuilder WithYouTubeCommentsList(List<YouTubeCommentDto> commentsList)
    {
        _event.YouTubeCommentsList = commentsList.Cast<IYouTubeCommentDto>().ToList();
        return this;
    }

    public FetchCompletedEvent Build()
    {
        return _event;
    }
}