using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
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
        
        CommentsFetchedEvent commentsFetchedEvent = new CommentsFetchedEvent
        {
            VideoId = videoId,
            PageToken = response.NextPageToken,
            CommentsFetchedCount = YouTubeCommentDtoList.Count,
            ReplyCount = YouTubeCommentDtoList.Sum(cmt => cmt.TotalReplyCount),
            YouTubeCommentsList = YouTubeCommentDtoList.Cast<IYouTubeCommentDto>().ToList()
        };

        return commentsFetchedEvent;
    }
}