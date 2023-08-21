using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class FetchCompletedEventBuilder
{
    private readonly CommentThreadListCompletedEvent _event;

    public FetchCompletedEventBuilder()
    {
        _event = new CommentThreadListCompletedEvent();
    }

    public FetchCompletedEventBuilder WithVideoId(string? videoId)
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
        _event.CommentsCount = count;
        return this;
    }

    public FetchCompletedEventBuilder WithReplyCount(int count)
    {
        _event.ReplyCount = count;
        return this;
    }
    
    public FetchCompletedEventBuilder WithCommentIds(List<string>? commentIds)
    {
        _event.CommentIds = commentIds;
        return this;
    }

    public FetchCompletedEventBuilder WithYouTubeCommentsList(List<YouTubeCommentDto> commentsList)
    {
        _event.YouTubeCommentsList = commentsList.Cast<IYouTubeCommentDto>().ToList();
        return this;
    }

    public CommentThreadListCompletedEvent Build()
    {
        return _event;
    }
}