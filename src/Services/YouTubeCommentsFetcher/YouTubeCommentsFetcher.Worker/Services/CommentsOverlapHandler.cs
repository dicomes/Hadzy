using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsOverlapHandler : ICommentsOverlapHandler
    {
        private readonly double _overlappingThreshold = 0.5;
        private readonly ILogger<CommentsOverlapHandler> _logger;

        public CommentsOverlapHandler(ILogger<CommentsOverlapHandler> logger)
        {
            _logger = logger;
        }

        public OverlapResult HandleOverlaps(CommentThreadListResponse response, List<string> storedCommentIds)
        {
            var (filteredResponse, overlappingCount) = RemoveOverlappingComments(response, storedCommentIds);
            bool shouldStopFetching = ShouldStopFetching(overlappingCount, response.Items.Count);

            return new OverlapResult
            {
                UpdatedResponse = filteredResponse,
                OverlappingCount = overlappingCount,
                ShouldStopFetching = shouldStopFetching
            };
        }
        
        private (CommentThreadListResponse FilteredResponse, int OverlappingCount) RemoveOverlappingComments(CommentThreadListResponse response, List<string> storedCommentIds)
        {
            if (storedCommentIds == null || storedCommentIds.Count == 0 || response.Items.Count == 0)
            {
                return (response, 0);
            }

            var originalCount = response.Items.Count;
            var storedIdsSet = new HashSet<string>(storedCommentIds);
            
            // Filter out items that are in the storedIdsSet in-place
            var filteredItems = response.Items.Where(item => !storedIdsSet.Contains(item.Snippet.TopLevelComment.Id)).ToList();
            response.Items = filteredItems;
            
            int overlappingCount = originalCount - response.Items.Count;
            
            return (response, overlappingCount);
        }

        private bool ShouldStopFetching(int overlappingCount, int totalResponseCount)
        {
            double overlapPercentage = CalculateOverlapPercentage(overlappingCount, totalResponseCount);
            return overlapPercentage > _overlappingThreshold;
        }

        private double CalculateOverlapPercentage(int overlappingCount, int totalResponseCount)
        {
            return (double)overlappingCount / totalResponseCount;
        }
    }

