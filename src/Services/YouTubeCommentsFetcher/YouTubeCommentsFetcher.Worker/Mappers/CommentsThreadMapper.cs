using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Builders;
using YouTubeCommentsFetcher.Worker.Contracts;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Mappers
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
            List<YouTubeCommentDto> youTubeCommentDtoList = _mapper.Map<List<YouTubeCommentDto>>(response.Items.Select(ct => ct.Snippet));
        
            CommentThreadListCompletedEvent commentThreadListCompletedEvent = new FetchCompletedEventBuilder(videoId)
                .WithPageToken(response.NextPageToken)
                .WithCommentsFetchedCount((ulong)youTubeCommentDtoList.LongCount())
                .WithReplyCount((uint)youTubeCommentDtoList.Sum(cmt => cmt.TotalReplyCount))
                .WithYouTubeCommentsList(youTubeCommentDtoList)
                .Build();

            return commentThreadListCompletedEvent;
        }
    }
}