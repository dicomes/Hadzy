using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services.Transformer;

public class CommentThreadToDtoTransformer : ICommentTransformer
{
    private readonly IMapper _mapper;

    public CommentThreadToDtoTransformer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ICommentsFetchedEvent Transform(string videoId, CommentThreadListResponse response)
    {
        var youTubeComments = _mapper.Map<List<IYouTubeCommentDto>>(response.Items.Select(ct => ct.Snippet.TopLevelComment));
        return new CommentsFetchedEvent
        {
            VideoId = videoId,
            PageToken = response.NextPageToken,
            YouTubeCommentsList = youTubeComments
        };
    }
}