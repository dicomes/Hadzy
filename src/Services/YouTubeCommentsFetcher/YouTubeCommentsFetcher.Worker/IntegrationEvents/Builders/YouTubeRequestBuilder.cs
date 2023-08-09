using Google.Apis.YouTube.v3;

namespace YouTubeCommentsFetcher.Worker.IntegrationEvents.Builders;

public class YouTubeRequestBuilder
{
    private CommentThreadsResource.ListRequest _request;

    public YouTubeRequestBuilder(YouTubeService youtubeService, string properties)
    {
        _request = youtubeService.CommentThreads.List(properties);
    }
    
    public YouTubeRequestBuilder SetVideoId(string videoId)
    {
        _request.VideoId = videoId;
        return this;
    }
    
    public YouTubeRequestBuilder SetMaxResults(int maxResults)
    {
        _request.MaxResults = maxResults;
        return this;
    }
    
    public YouTubeRequestBuilder SetPageToken(string pageToken)
    {
        if (!string.IsNullOrEmpty(pageToken))
        {
            _request.PageToken = pageToken;
        }
        
        return this;
    }
    
    public CommentThreadsResource.ListRequest Build()
    {
        return _request;
    }
}