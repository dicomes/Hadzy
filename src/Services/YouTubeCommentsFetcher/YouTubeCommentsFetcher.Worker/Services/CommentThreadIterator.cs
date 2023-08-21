using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

 public class CommentThreadIterator : ICommentThreadIterator
    {
        private readonly ICommentsThreadMapper _commentsThreadMapper;
        private readonly ICommentsOverlapHandler _overlapService;
        private readonly ILogger<CommentThreadIterator> _logger;
        private readonly IYouTubeFetcherService _fetcherService;
        private string? _pageToken;

        public CommentThreadIterator(
            ICommentsThreadMapper commentsThreadMapper,
            ICommentsOverlapHandler overlapService,
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
                RemoveOverlappingCommentsFromResponse(response, fetchParams);
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

        private void RemoveOverlappingCommentsFromResponse(CommentThreadListResponse commentThreadList, FetchParams fetchParams)
        {
            var overlapResult = _overlapService.HandleOverlaps(commentThreadList.Items.ToList(), fetchParams.PreviouslyFetchedCommentIds);
            commentThreadList.Items = overlapResult.UpdatedCommentList;
            LogOverlappingWarnings(overlapResult, fetchParams);
            UpdatePageToken(overlapResult, commentThreadList);
        }

        private void LogOverlappingWarnings(OverlapResult overlapResult, FetchParams fetchParams)
        {
            if (overlapResult.OverlappingCount > 0)
            {
                _logger.LogWarning("{Source}: Overlapping occurred {OverlappingCount} out of {BaseCommentCount} for VideoId: {VideoId}.", 
                    GetType().Name, overlapResult.OverlappingCount, overlapResult.BaseCommentCount, fetchParams.VideoId);
            }

            if (overlapResult.ShouldStopFetching)
            {
                _logger.LogWarning("{Source}: Overlapping threshold met for VideoId: {VideoId}. Overlapping Count: {OverlappingCount} out of {BaseCommentCount}. Fetching will stop.",
                    GetType().Name, fetchParams.VideoId, overlapResult.OverlappingCount, overlapResult.BaseCommentCount);
            }
        }

        private void UpdatePageToken(OverlapResult overlapResult, CommentThreadListResponse response)
        {
            _pageToken = overlapResult.ShouldStopFetching ? string.Empty : response.NextPageToken;
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

