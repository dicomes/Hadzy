using System.Net;
using AutoMapper;
using Google;
using Polly;
using Polly.Retry;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsService : ICommentsService
{
    private readonly IFetcherService _fetcherService;
    private readonly AsyncRetryPolicy _retryPolicy;
    private readonly IMapper _mapper;

    public CommentsService(IFetcherService fetcherService, IMapper mapper)
    {
        _fetcherService = fetcherService;
        _mapper = mapper;
        
        _retryPolicy = Policy
            .Handle<GoogleApiException>(e => e.HttpStatusCode == HttpStatusCode.InternalServerError)
            .Or<TimeoutException>()
            .RetryAsync(3);
    }

    public async Task<CommentsBatchDto> GetCommentsByVideoIdAsync(string videoId, int maxResults)
    {
        return await _retryPolicy.ExecuteAsync(async () => await FetchComments(videoId, maxResults));
    }

    private async Task<CommentsBatchDto> FetchComments(string videoId, int maxResults)
    {
        var response = await _fetcherService.GetCommentThreadList(videoId, maxResults);
        
        var youTubeComments = _mapper.Map<List<YouTubeComment>>(response.Items.Select(ct => ct.Snippet.TopLevelComment).ToList());
        var commentsBatchDto = new CommentsBatchDto
        {
            VideoId = videoId,
            YouTubeCommentsList = youTubeComments
        };
        return commentsBatchDto;
    }
}