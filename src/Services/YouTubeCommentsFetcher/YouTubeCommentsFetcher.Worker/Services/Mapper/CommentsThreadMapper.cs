using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models.DTO;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services.Mapper;

public class CommentsThreadMapper : ICommentsThreadMapper
{
    private readonly IMapper _mapper;

    public CommentsThreadMapper(IMapper mapper)
    {
        _mapper = mapper;
    }

    public FetchCompletedEvent MapToFetchCompletedEvent(string videoId, CommentThreadListResponse response)
    {
        // Extract the comments and map them
        List<YouTubeCommentDto> YouTubeCommentDtoList = _mapper.Map<List<YouTubeCommentDto>>(response.Items.Select(ct => ct.Snippet));

        FetchCompletedEventBuilder fetchCompletedEventBuilder = new FetchCompletedEventBuilder();
        FetchCompletedEvent fetchCompletedEvent = fetchCompletedEventBuilder
            .WithVideoId(videoId)
            .WithPageToken(response.NextPageToken)
            .WithCommentsFetchedCount(YouTubeCommentDtoList.Count)
            .WithReplyCount(YouTubeCommentDtoList.Sum(cmt => cmt.TotalReplyCount))
            .WithYouTubeCommentsList(YouTubeCommentDtoList)
            .Build();

        return fetchCompletedEvent;
    }
}