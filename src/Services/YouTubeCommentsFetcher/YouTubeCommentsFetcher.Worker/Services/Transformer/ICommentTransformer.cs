using Google.Apis.YouTube.v3.Data;
using SharedEventContracts;

namespace YouTubeCommentsFetcher.Worker.Services.Transformer;

public interface ICommentTransformer
{
    ICommentsFetchedEvent Transform(string videoId, CommentThreadListResponse response);
}