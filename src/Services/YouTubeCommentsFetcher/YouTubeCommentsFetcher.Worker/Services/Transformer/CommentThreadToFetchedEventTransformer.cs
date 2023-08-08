using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models.DTO;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services.Transformer;

public class CommentThreadToFetchedEventTransformer : ICommentTransformer
{
    private readonly IMapper _mapper;

    public CommentThreadToFetchedEventTransformer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public CommentsFetchedEvent Transform(string videoId, CommentThreadListResponse response)
    {
        // Extract the comments and map them
        List<YouTubeCommentDto> YouTubeCommentDtoList = _mapper.Map<List<YouTubeCommentDto>>(response.Items.Select(ct => ct.Snippet));

        CommentsFetchedEventBuilder commentsFetchedEventBuilder = new CommentsFetchedEventBuilder();
        CommentsFetchedEvent commentsFetchedEvent = commentsFetchedEventBuilder
            .WithVideoId(videoId)
            .WithPageToken(response.NextPageToken)
            .WithCommentsFetchedCount(YouTubeCommentDtoList.Count)
            .WithReplyCount(YouTubeCommentDtoList.Sum(cmt => cmt.TotalReplyCount))
            .WithYouTubeCommentsList(YouTubeCommentDtoList)
            .Build();

        return commentsFetchedEvent;
    }
}