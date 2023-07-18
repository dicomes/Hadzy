using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using YouTubeVideoFetcher.MinimalApi.Endpoints;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
using YouTubeVideoFetcher.Services;

namespace YouTubeVideoFetcher.UnitTests;

public class VideoEndpointsTests
{
    private readonly Mock<IVideoService> _videoServiceMock;
    private readonly WebApplication _app;

    public VideoEndpointsTests()
    {
        _videoServiceMock = new Mock<IVideoService>();
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton(_videoServiceMock.Object);

        _app = builder.Build();
        _app.ConfigureVideoEndpoints();
    }

    [Fact]
    public async Task GetVideo_ReturnsOk_WhenVideoExists()
    {
        // Arrange
        var videoId = "existentVideoId";
        var expectedVideoDto = new YouTubeVideoDto { id = videoId };

        _videoServiceMock.Setup(s => s.GetVideoByIdAsync(videoId)).ReturnsAsync(expectedVideoDto);

        // Act
        using var client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
        var response = await client.GetAsync($"/video-fetcher/api/v1/video{videoId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetVideo_ReturnsNotFound_WhenVideoDoesNotExist()
    {
        // Arrange
        var videoId = "nonexistentVideoId";

        _videoServiceMock.Setup(s => s.GetVideoByIdAsync(videoId)).ThrowsAsync(new VideoNotFoundException(String.Empty));

        // Act
        using var client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
        var response = await client.GetAsync($"/video-fetcher/api/v1/video{videoId}");


        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

}

