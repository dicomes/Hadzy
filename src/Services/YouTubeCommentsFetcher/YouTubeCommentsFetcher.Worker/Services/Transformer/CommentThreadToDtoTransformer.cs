using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;

namespace YouTubeCommentsFetcher.Worker.Services.Transformer;

public class CommentThreadToDtoTransformer : ICommentTransformer
{
    private readonly IMapper _mapper;

    public CommentThreadToDtoTransformer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public CommentsFetchedEvent Transform(string videoId, CommentThreadListResponse response)
    {
        // Extract the comments and map them
        List<YouTubeCommentDto> youTubeCommentDtos = _mapper.Map<List<YouTubeCommentDto>>(response.Items.Select(ct => ct.Snippet.TopLevelComment));
        
        CommentsFetchedEvent commentsFetchedEvent = new CommentsFetchedEvent
        {
            VideoId = videoId,
            PageToken = response.NextPageToken,
            YouTubeCommentsList = youTubeCommentDtos.Cast<IYouTubeCommentDto>().ToList()
        };

        return commentsFetchedEvent;
    }
}