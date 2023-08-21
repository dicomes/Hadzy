using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;
using YouTubeCommentsFetcher.Worker.Models.DTO;
using YouTubeCommentsFetcher.Worker.Services.Interfaces;

namespace YouTubeCommentsFetcher.Worker.Services.Mappers
{
    public class CommentsThreadMapper : ICommentsThreadMapper
    {
        private readonly IMapper _mapper;

        public CommentsThreadMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public CommentThreadListCompletedEvent ToBatchCompletedEvent(string? videoId, CommentThreadListResponse response)
        {
            // Extract the comments and map them
            List<YouTubeCommentDto> youTubeCommentDtoList = _mapper.Map<List<YouTubeCommentDto>>(response.Items.Select(ct => ct.Snippet));
        
            CommentThreadListCompletedEvent commentThreadListCompletedEvent = new FetchCompletedEventBuilder()
                .WithVideoId(videoId)
                .WithPageToken(response.NextPageToken)
                .WithCommentsFetchedCount(youTubeCommentDtoList.Count)
                .WithReplyCount(youTubeCommentDtoList.Sum(cmt => cmt.TotalReplyCount))
                .WithYouTubeCommentsList(youTubeCommentDtoList)
                .Build();

            return commentThreadListCompletedEvent;
        }
    }
}