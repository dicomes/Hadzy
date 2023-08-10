using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsIterator : ICommentsIterator
{
    private readonly IYouTubeFetcherService _fetcherService;
    private readonly ICommentMapper _mapper;
    private string _pageToken;

    public CommentsIterator(
        IYouTubeFetcherService fetcherService, ICommentMapper mapper)
    {
        _fetcherService = fetcherService;
        _mapper = mapper;
    }

    public async Task<FetchCompletedEvent> Next(FetchSettings fetchSettings)
    {
        var response = await _fetcherService.FetchAsync(fetchSettings);
        _pageToken = response.NextPageToken;
        return _mapper.Map(fetchSettings.VideoId, response);
    }

    public bool HasNext()
    {
        return !string.IsNullOrEmpty(_pageToken);
    }
}
