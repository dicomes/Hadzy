using System.Net;
using Google;
using SharedEventContracts;
using YouTubeCommentsFetcher.Worker.Exceptions;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Fetcher;
using YouTubeCommentsFetcher.Worker.Services.Transformer;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsService : ICommentsService
{
    private readonly IFetcherService _fetcherService;
    private readonly ICommentTransformer _transformer;

    public CommentsService(IFetcherService fetcherService, ICommentTransformer transformer)
    {
        _fetcherService = fetcherService;
        _transformer = transformer;
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
                throw new CommentsAccessForbiddenException(e.Message);
            }
                    
            if (e.HttpStatusCode == HttpStatusCode.NotFound)
            {
                throw new CommentsNotFoundException(e.Message);
            }

            throw;
        }
    }
}