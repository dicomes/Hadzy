using IntegrationEventsContracts;
using YouTubeCommentsFetcher.Worker.Builders;
using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models;

namespace YouTubeCommentsFetcher.Worker.Services;

public class EventsManager : IEventsManager
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<EventsManager> _logger;
        private readonly ICommentFetchIterator _commentFetchIterator;

        public EventsManager(
            IEventPublisher eventPublisher,
            ILogger<EventsManager> logger,
            ICommentFetchIterator commentFetchIterator)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
            _commentFetchIterator = commentFetchIterator;
        }

        public async Task IterateCommentsAndPublishEventsAsync(
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
                    
                    if (_commentFetchIterator.HasNext())
                    {
                        await PublishCommentThreadEvent(commentThreadListCompletedEvent);
                    }
                    
                    if (!_commentFetchIterator.HasNext())
                    {
                        await PublishFetchCompletedEvent(commentThreadListCompletedEvent);
                    }

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
                await PublishFetchInfoEvent(videoId, fetchParams.PageToken, new CommentThreadListCompletedEvent(videoId), new List<string>(), FetchStatus.Failed.ToString(), false);
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
            CommentThreadListCompletedEvent commentThreadListCompletedEvent)
        {
            await _eventPublisher.PublishEvent(commentThreadListCompletedEvent);
        }
        
        private async Task PublishFetchCompletedEvent(CommentThreadListCompletedEvent commentThreadListCompletedEvent)
        {
            if (commentThreadListCompletedEvent.VideoId != null)
            {
                var fetchCompletedEvent = new FetchCompletedEvent()
                {
                    VideoId = commentThreadListCompletedEvent.VideoId,
                    FirstCommentId = commentThreadListCompletedEvent.YouTubeCommentsList?.LastOrDefault()?.Id ?? default
                };
                await _eventPublisher.PublishEvent(fetchCompletedEvent);
            }
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
