using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services;

 public class CommentFetchIterator : ICommentFetchIterator
    {
        private readonly ICommentsThreadMapper _commentsThreadMapper;
        private readonly ICommentsOverlapHandler _overlapService;
        private readonly ILogger<CommentFetchIterator> _logger;
        private readonly IYouTubeFetcher _fetcher;
        private string? _pageToken;

        public CommentFetchIterator(
            ICommentsThreadMapper commentsThreadMapper,
            ICommentsOverlapHandler overlapService,
            ILogger<CommentFetchIterator> logger,
            IYouTubeFetcher fetcher
        )
        {
            _commentsThreadMapper = commentsThreadMapper;
            _overlapService = overlapService;
            _logger = logger;
            _fetcher = fetcher;
        }

        public async Task<CommentThreadListCompletedEvent> Next(FetchParams fetchParams)
        {
            var response = await FetchCommentThreadList(fetchParams);
            var commentIds = ExtractCommentIds(response);

            if (!IsFirstFetch(fetchParams))
            {
                RemoveOverlappingCommentsFromResponse(ref response, fetchParams);
            }
            else
            {
                _pageToken = response.NextPageToken;
            }

            return CreateBatchCompletedEvent(fetchParams.VideoId, response, commentIds);
        }

        private async Task<CommentThreadListResponse> FetchCommentThreadList(FetchParams fetchParams)
        {
            return await _fetcher.FetchAsync(fetchParams);
        }

        private List<string>? ExtractCommentIds(CommentThreadListResponse response)
        {
            return response.Items.Select(ct => ct.Snippet.TopLevelComment.Id).ToList();
        }

        private bool IsFirstFetch(FetchParams fetchParams)
        {
            return fetchParams.FirstCommentsFromPreviousBatch.Count == 0;
        }

        private void RemoveOverlappingCommentsFromResponse(ref CommentThreadListResponse commentThreadList, FetchParams fetchParams)
        {
            var overlapResult = _overlapService.HandleOverlaps(commentThreadList.Items.ToList(), fetchParams.FirstCommentsFromPreviousBatch);
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
            // Overlap occurs only if comments have already been fetched.
            // This means the fetching has reached the last comment before and the pageToken was empty.
            _pageToken = overlapResult.ShouldStopFetching ? string.Empty : response.NextPageToken;
        }

        private CommentThreadListCompletedEvent CreateBatchCompletedEvent(string? videoId, CommentThreadListResponse response, List<string>? commentIds)
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

