using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Fetcher;
using YouTubeCommentsFetcher.Worker.Services.Transformer;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsService : ICommentsService
{
    private readonly IFetcherService _fetcherService;
    private readonly ICommentTransformer _transformer;
    private readonly ILogger<CommentsService> _logger;

    public CommentsService(IFetcherService fetcherService, ICommentTransformer transformer, ILogger<CommentsService> logger)
    {
        _fetcherService = fetcherService;
        _transformer = transformer;
        _logger = logger;
    }

    public async Task<ICommentsFetchedEvent> GetCommentsFetchedEventByVideoIdAsync(FetchSettings fetchSettings)
    {
        var response = await _fetcherService.FetchAsync(fetchSettings);
        return _transformer.Transform(fetchSettings.VideoId, response);
    }
}