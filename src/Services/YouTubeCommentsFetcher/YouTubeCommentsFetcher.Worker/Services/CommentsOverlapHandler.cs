using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services;

public class CommentsOverlapHandler : ICommentsOverlapHandler
    {
        private readonly double _overlappingThreshold = 0.1;

        public OverlapResult HandleOverlaps(IList<CommentThread> items, List<string> storedCommentIds)
        {
            var (filteredItems, overlappingCount) = RemoveOverlappingComments(items.ToList(), storedCommentIds);
            bool shouldStopFetching = ShouldStopFetching(overlappingCount, items.Count);

            return new OverlapResult
            {
                UpdatedCommentList = filteredItems,
                OverlappingCount = overlappingCount,
                BaseCommentCount = items.Count,
                ShouldStopFetching = shouldStopFetching
            };
        }
        
        private (IList<CommentThread> FilteredItems, int OverlappingCount) RemoveOverlappingComments(IList<CommentThread> items, List<string> storedCommentIds)
        {
            if (storedCommentIds.Count == 0 || items.Count == 0)
            {
                return (items, 0);
            }

            var storedIdsSet = new HashSet<string>(storedCommentIds);
            
            // Filter out items that are in the storedIdsSet in-place
            var filteredItems = items.Where(item => !storedIdsSet.Contains(item.Snippet.TopLevelComment.Id)).ToList();
            var overlappingCount = items.Count - filteredItems.Count;
            return (filteredItems, overlappingCount);
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

