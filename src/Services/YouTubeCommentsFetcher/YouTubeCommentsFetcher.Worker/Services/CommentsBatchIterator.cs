using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIterator : ICommentsIterator
{
    private readonly IYouTubeFetcherService _fetcherService;
    private readonly ICommentsThreadMapper _mapper;
    private string? _pageToken;

    public CommentsIterator(
        IYouTubeFetcherService fetcherService, ICommentsThreadMapper mapper)
    {
        _fetcherService = fetcherService;
        _mapper = mapper;
    }

    public async Task<FetchBatchCompletedEvent> Next(FetchSettings fetchSettings)
    {
        var response = await _fetcherService.FetchAsync(fetchSettings);
        _pageToken = response.NextPageToken;
        return _mapper.MapToFetchCompletedEvent(fetchSettings.VideoId, response);
    }

    public bool HasNext()
    {
        return !string.IsNullOrEmpty(_pageToken);
    }
}
