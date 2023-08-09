using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIterator
{
    private readonly string _videoId;
    private readonly IYouTubeFetcherService _fetcherService;
    private string _currentPageToken;
    private readonly ICommentMapper _mapper;


    public CommentsIterator(
        string videoId,
        IYouTubeFetcherService fetcherService,
        string initialPageToken = null)
    {
        _videoId = videoId;
        _fetcherService = fetcherService;
        _currentPageToken = initialPageToken;
    }

    public async Task<FetchCompletedEvent> Next()
    {
        var fetchSettings = new FetchSettings(_videoId, _currentPageToken);
        var response = await _fetcherService.FetchAsync(fetchSettings);

        _currentPageToken = response.NextPageToken;

        return _mapper.Map(fetchSettings.VideoId, response);
    }

    public bool HasNext()
    {
        return !string.IsNullOrEmpty(_currentPageToken);
    }
}
