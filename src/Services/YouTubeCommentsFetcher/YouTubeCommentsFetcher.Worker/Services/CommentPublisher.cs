using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentPublisher : ICommentPublisher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<CommentPublisher> _logger;
        private readonly ICommentFetchIterator _commentFetchIterator;

        public CommentPublisher(
            IEventPublisher eventPublisher,
            ILogger<CommentPublisher> logger,
            ICommentFetchIterator commentFetchIterator)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
            _commentFetchIterator = commentFetchIterator;
        }

        public async Task IterateAndPublishCommentsAsync(
            string? videoId, string? pageToken, List<string> commentIds)
        {
            ulong totalCommentsFetched = 0;
            uint totalReplies = 0;
            var fetchParams = new FetchParams(videoId, pageToken, commentIds);
            List<string>? firstIterationCommentIds = new List<string>();
            bool firstIteration = true;

            try
            {
                do
                {
                    var commentThreadListCompletedEvent = await Batch(fetchParams);
                    await PublishCommentThreadEvent(commentThreadListCompletedEvent, !_commentFetchIterator.HasNext());

                    firstIteration = HandleFirstIteration(firstIteration, commentThreadListCompletedEvent,
                        ref firstIterationCommentIds);
                    
                    await PublishFetchInfoEvent(
                        videoId, string.Empty, commentThreadListCompletedEvent, firstIterationCommentIds,
                        GetStatus(_commentFetchIterator.HasNext()), !_commentFetchIterator.HasNext());
                    
                    IncrementComments(ref totalCommentsFetched, ref totalReplies, commentThreadListCompletedEvent);

                    fetchParams.PageToken = commentThreadListCompletedEvent.NextPageToken;

                } while (_commentFetchIterator.HasNext());

                _logger.LogInformation("{Source}: Completed fetching all comments for VideoId: {VideoId}. Total Comments: {TotalComments}. Total Replies: {TotalReplies}.",
                    GetType().Name, videoId, totalCommentsFetched, totalReplies);
            }
            catch
            {
                await PublishFetchInfoEvent(videoId, fetchParams.PageToken, new CommentThreadListCompletedEvent(), new List<string>(), FetchStatus.Failed.ToString(), false);
                throw;
            }
        }

        private void IncrementComments(ref ulong totalCommentsFetched, ref uint totalReplies, CommentThreadListCompletedEvent commentThreadListCompletedEvent )
        {
            totalCommentsFetched += commentThreadListCompletedEvent.CommentsCount;
            totalReplies += commentThreadListCompletedEvent.ReplyCount;
        }

        private string GetStatus(bool hasNext)
        {
            return hasNext ? FetchStatus.InProgress.ToString() : FetchStatus.Done.ToString();
        }

        private bool HandleFirstIteration(bool firstIteration, CommentThreadListCompletedEvent commentThreadListCompletedEvent, ref List<string>? firstIterationCommentIds)
        {
            if (firstIteration)
            {
                firstIterationCommentIds = commentThreadListCompletedEvent.CommentIds;
                firstIteration = false;
            }

            return firstIteration;
        }

        private async Task<CommentThreadListCompletedEvent> Batch(FetchParams fetchParams)
        {
            var fetchBatchCompletedEvent = await _commentFetchIterator.Next(fetchParams);
            _logger.LogInformation("{Source}: Batch fetch completed for VideoId: {VideoId}. NextPageToken: {PageToken}. Comments in batch: {CommentsCount}. Replies in batch: {RepliesCount}.",
                GetType().Name, fetchParams.VideoId, fetchParams.PageToken, fetchBatchCompletedEvent.CommentsCount, fetchBatchCompletedEvent.ReplyCount);
            return fetchBatchCompletedEvent;
        }

        private async Task PublishCommentThreadEvent(
            CommentThreadListCompletedEvent commentThreadListCompletedEvent, bool fetchCompletedTillFirstComment)
        {
            if (fetchCompletedTillFirstComment)
            {
                commentThreadListCompletedEvent.FirstCommentId =
                    commentThreadListCompletedEvent.YouTubeCommentsList.Last().Id;
            }
            await _eventPublisher.PublishEvent(commentThreadListCompletedEvent);
        }

        private async Task PublishFetchInfoEvent(
            string videoId, string? pageToken, CommentThreadListCompletedEvent commentThreadListCompletedEvent,
            List<string>? firstIterationCommentIds, string fetchStatus, bool fetchCompletedTillFirstComment)
        {
            var fetchInfoChangedEvent = new FetchInfoChangedEventBuilder(videoId)
                .WithPageToken(pageToken)
                .WithCommentsFetchedCount(commentThreadListCompletedEvent.CommentsCount)
                .WithReplyCount(commentThreadListCompletedEvent.ReplyCount)
                .WithCommentIds(firstIterationCommentIds)
                .WithStatus(fetchStatus)
                .WithCompletedTillFirstComment(fetchCompletedTillFirstComment)
                .Build();

            await _eventPublisher.PublishEvent(fetchInfoChangedEvent);
        }
    }