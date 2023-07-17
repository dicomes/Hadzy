using Google;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Polly;
using Polly.Retry;
using System.Net;
using AutoMapper;
using YouTubeVideoFetcher.MinimalApi;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;

namespace YouTubeVideoFetcher.Services
{
    public class VideoService : IVideoService
    {
        private readonly YouTubeService _youtubeService;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly IMapper _mapper;

        public VideoService(YouTubeService youtubeService, IMapper mapper)
        {
            _youtubeService = youtubeService;
            _mapper = mapper;

            _retryPolicy = Policy
                .Handle<GoogleApiException>(e => e.HttpStatusCode == HttpStatusCode.InternalServerError)
                .Or<TimeoutException>()
                .RetryAsync(3);
        }

        public async Task<YouTubeVideoDto> GetVideoByIdAsync(string videoId)
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

                    Video video = response.Items[0];
                    YouTubeVideoDto videoDto = _mapper.Map<YouTubeVideoDto>(video);
                    
                    return videoDto;
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

                    throw;
                }
            });
        }

    }
}