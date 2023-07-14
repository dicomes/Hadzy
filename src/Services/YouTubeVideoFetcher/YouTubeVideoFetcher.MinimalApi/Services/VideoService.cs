using Google;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Polly;
using Polly.Retry;
using System.Net;

using YouTubeVideoFetcher.MinimalApi.Exceptions;

namespace YouTubeVideoFetcher.Services
{
    public class VideoService : IVideoService
    {
        private readonly YouTubeService _youtubeService;
        private readonly AsyncRetryPolicy _retryPolicy;

        public VideoService(YouTubeService youtubeService)
        {
            _youtubeService = youtubeService;

            _retryPolicy = Policy
                .Handle<GoogleApiException>(e => e.HttpStatusCode == HttpStatusCode.InternalServerError)
                .Or<TimeoutException>()
                .RetryAsync(3);
        }

        public async Task<Video> GetVideoByIdAsync(string videoId)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    var request = _youtubeService.Videos.List("snippet,statistics");
                    request.Id = videoId;

                    var response = await request.ExecuteAsync();

                    if (response.Items.Count == 0)
                    {
                        throw new VideoNotFoundException($"Video with ID '{videoId}' not found");
                    }

                    return response.Items[0];
                }
                catch (GoogleApiException e)
                {
                    if (e.HttpStatusCode == HttpStatusCode.Forbidden)
                    {
                        throw new VideoAccessForbiddenException(e.Message);
                    }
                    
                    if (e.HttpStatusCode == HttpStatusCode.BadRequest)
                    {
                        throw new VideoBadRequestException(e.Message);
                    }

                    // If the error is something other than forbidden access or bad request, just rethrow the original exception.
                    throw;
                }
            });
        }

    }
}