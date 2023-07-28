using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Services.Transformer;

public interface ICommentTransformer
{
    CommentsBatchDto Transform(string videoId, CommentThreadListResponse response);
}