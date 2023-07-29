using System.Net;
using Google;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Exceptions;
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
        try
        {
            var response = await _fetcherService.FetchAsync(fetchSettings);
            return _transformer.Transform(fetchSettings.VideoId, response);
        }
        catch (GoogleApiException e)
        {
            if (e.HttpStatusCode == HttpStatusCode.Forbidden)
            {
                _logger.LogWarning("Access forbidden while fetching comments for video ID {VideoId}. Error: {ErrorMessage}", fetchSettings.VideoId, e.Message);
                throw new CommentsAccessForbiddenException(e.Message);
            }
                    
            if (e.HttpStatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Video not found while fetching comments for video ID {VideoId}. Error: {ErrorMessage}", fetchSettings.VideoId, e.Message);
                throw new VideoNotFoundException(e.Message);
            }
            
            _logger.LogError(e, "An unexpected error occurred while fetching comments for video ID {VideoId}", fetchSettings.VideoId);
            throw;
        }
    }
}