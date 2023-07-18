using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using Moq;
using Xunit;
using YouTubeVideoFetcher.MinimalApi;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
using YouTubeVideoFetcher.Services;

namespace YouTubeVideoFetcher.UnitTests;

public class VideoServiceTests
{
    private readonly Mock<IFetcherService> _fetcherServiceMock;
    private readonly IMapper _mapper;
    private readonly VideoService _videoService;

    public VideoServiceTests()
    {
        _fetcherServiceMock = new Mock<IFetcherService>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingConfig>()).CreateMapper();
        _videoService = new VideoService(_fetcherServiceMock.Object, _mapper);
    }

    [Fact]
    public async Task GetVideoByIdAsync_ReturnsVideo_WhenVideoExists()
    {
        // Arrange
        var videoId = "existentVideoId";
        var expectedVideo = new Video { Id = videoId };
        var expectedVideoDto = _mapper.Map<YouTubeVideoDto>(expectedVideo);

        var videoListResponse = new VideoListResponse();
        videoListResponse.Items = new List<Video> { expectedVideo };

        _fetcherServiceMock.Setup(s => s.GetVideoListByIdAsync(videoId)).ReturnsAsync(videoListResponse);

        // Act
        var videoDto = await _videoService.GetVideoByIdAsync(videoId);

        // Assert
        Assert.Equal(expectedVideoDto.id, videoDto.id);
    }
    
    [Fact]
    public async Task GetVideoByIdAsync_ThrowsVideoNotFoundException_WhenVideoDoesNotExist()
    {
        // Arrange
        var videoId = "nonExistentVideoId";

        var videoListResponse = new VideoListResponse();
        videoListResponse.Items = new List<Video>();

        _fetcherServiceMock.Setup(s => s.GetVideoListByIdAsync(videoId)).ReturnsAsync(videoListResponse);

        // Act and Assert
        await Assert.ThrowsAsync<VideoNotFoundException>(() => _videoService.GetVideoByIdAsync(videoId));
    }

}
