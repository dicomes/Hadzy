using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

 public class CommentThreadIterator : ICommentThreadIterator
    {
        private readonly ICommentsThreadMapper _commentsThreadMapper;
        private readonly ICommentsOverlapHandler _overlapService; // Renamed and refactored to a service
        private readonly ILogger<CommentThreadIterator> _logger;
        private readonly IYouTubeFetcherService _fetcherService;
        private string? _pageToken;

        public CommentThreadIterator(
            ICommentsThreadMapper commentsThreadMapper,
            ICommentsOverlapHandler overlapService, // Use the new service
            ILogger<CommentThreadIterator> logger,
            IYouTubeFetcherService fetcherService
        )
        {
            _commentsThreadMapper = commentsThreadMapper;
            _overlapService = overlapService;
            _logger = logger;
            _fetcherService = fetcherService;
        }

        public async Task<CommentThreadListCompletedEvent> Next(FetchParams fetchParams)
        {
            var response = await FetchCommentThreadList(fetchParams);
            var commentIds = ExtractCommentIds(response);

            if (!IsFirstFetch(fetchParams))
            {
                HandleOverlappingComments(response, fetchParams);
            }
            else
            {
                _pageToken = response.NextPageToken;
            }

            return CreateBatchCompletedEvent(fetchParams.VideoId, response, commentIds);
        }

        private async Task<CommentThreadListResponse> FetchCommentThreadList(FetchParams fetchParams)
        {
            return await _fetcherService.FetchAsync(fetchParams);
        }

        private List<string> ExtractCommentIds(CommentThreadListResponse response)
        {
            return response.Items.Select(ct => ct.Snippet.TopLevelComment.Id).ToList();
        }

        private bool IsFirstFetch(FetchParams fetchParams)
        {
            return fetchParams.PreviouslyFetchedCommentIds.Count == 0;
        }

        private void HandleOverlappingComments(CommentThreadListResponse response, FetchParams fetchParams)
        {
            var overlapResult = _overlapService.HandleOverlaps(response, fetchParams.PreviouslyFetchedCommentIds);
            response = overlapResult.UpdatedResponse;

            if (overlapResult.ShouldStopFetching)
            {
                _logger.LogWarning("{Source}: Overlapping occurred for VideoId: {VideoId}. Stopping fetch.", GetType().Name, fetchParams.VideoId);
                _pageToken = null;
            }
            else
            {
                _pageToken = response.NextPageToken;
            }
        }

        private CommentThreadListCompletedEvent CreateBatchCompletedEvent(string videoId, CommentThreadListResponse response, List<string> commentIds)
        {
            var fetchBatchCompletedEvent = _commentsThreadMapper.ToBatchCompletedEvent(videoId, response);
            fetchBatchCompletedEvent.CommentIds = commentIds;
            return fetchBatchCompletedEvent;
        }

        public bool HasNext()
        {
            return !string.IsNullOrEmpty(_pageToken);
        }
    }

