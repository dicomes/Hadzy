using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class IntegrationEventsManager : IIntegrationEventsManager
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<IntegrationEventsManager> _logger;
        private readonly ICommentThreadIterator _commentThreadIterator;

        public IntegrationEventsManager(
            IEventPublisher eventPublisher,
            ILogger<IntegrationEventsManager> logger,
            ICommentThreadIterator commentThreadIterator)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
            _commentThreadIterator = commentThreadIterator;
        }

        public async Task ProcessCommentsAndPublishFetchedEventsAsync(string videoId, string? pageToken, List<string> commentIds)
        {
            int totalCommentsFetched = 0;
            int totalReplies = 0;
            var fetchParams = new FetchParams(videoId, pageToken, commentIds);
            List<string> firstIterationCommentIds = new List<string>();
            bool firstIteration = true;

            do
            {
                var commentThreadListCompletedEvent = await Batch(fetchParams, videoId, pageToken);
                await PublishFetchCommentThreadListCompletedEvent(commentThreadListCompletedEvent);

                if (firstIteration)
                {
                    firstIterationCommentIds = commentThreadListCompletedEvent.CommentIds;
                    firstIteration = false;
                }
                
                // Refactor to one completed event
                await PublishFetchInfoChangedEvent(fetchParams, videoId, commentThreadListCompletedEvent, firstIterationCommentIds);

                totalCommentsFetched += commentThreadListCompletedEvent.CommentsCount;
                totalReplies += commentThreadListCompletedEvent.ReplyCount;

                fetchParams.PageToken = commentThreadListCompletedEvent.NextPageToken;

            } while (_commentThreadIterator.HasNext());

            _logger.LogInformation("{Source}: Completed processing for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.", GetType().Name, videoId, totalCommentsFetched, totalReplies);
        }

        private async Task<CommentThreadListCompletedEvent> Batch(FetchParams fetchParams, string videoId, string? pageToken)
        {
            var fetchBatchCompletedEvent = await _commentThreadIterator.Next(fetchParams);
            _logger.LogInformation("{Source}: Fetching batch processed for VideoId: {VideoId}. NextPageToken: {NextPageToken}. Batch size: {CommentsCount}. Replies count: {RepliesCount}.",
                GetType().Name, videoId, pageToken, fetchBatchCompletedEvent.CommentsCount, fetchBatchCompletedEvent.ReplyCount);
            return fetchBatchCompletedEvent;
        }

        private async Task PublishFetchCommentThreadListCompletedEvent(CommentThreadListCompletedEvent commentThreadListCompletedEvent)
        {
            await _eventPublisher.PublishEvent(commentThreadListCompletedEvent);
        }

        private async Task PublishFetchInfoChangedEvent(FetchParams fetchParams, string videoId, CommentThreadListCompletedEvent commentThreadListCompletedEvent, List<string> firstIterationCommentIds)
        {
            var fetchInfoChangedEvent = new FetchInfoChangedEventBuilder()
                .WithVideoId(videoId)
                .WithPageToken(fetchParams.PageToken)
                .WithCommentsFetchedCount(commentThreadListCompletedEvent.CommentsCount)
                .WithReplyCount(commentThreadListCompletedEvent.ReplyCount)
                .WithCommentIds(firstIterationCommentIds)
                .WithStatus(_commentThreadIterator.HasNext() ? FetchStatus.InProgress.ToString() : FetchStatus.Done.ToString())
                .Build();

            await _eventPublisher.PublishEvent(fetchInfoChangedEvent);
        }
    }
