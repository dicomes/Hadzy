using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Builders;

public class FetchCompletedEventBuilder
{
    private readonly CommentThreadListCompletedEvent _event;

    public FetchCompletedEventBuilder(string videoId)
    {
        _event = new CommentThreadListCompletedEvent(videoId);
    }
    
    public FetchCompletedEventBuilder WithPageToken(string pageToken)
    {
        _event.NextPageToken = pageToken;
        return this;
    }

    public FetchCompletedEventBuilder WithCommentsFetchedCount(ulong count)
    {
        _event.CommentsCount = count;
        return this;
    }

    public FetchCompletedEventBuilder WithReplyCount(uint count)
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