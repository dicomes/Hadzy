using System.Net;
using AutoMapper;
using Google;
using Google.Apis.YouTube.v3.Data;
using YouTubeVideoFetcher.MinimalApi.Exceptions;
using YouTubeVideoFetcher.MinimalApi.Models;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
using YouTubeVideoFetcher.Services;

namespace YouTubeVideoFetcher.MinimalApi.Endpoints;

public static class VideoEndpoints
{
    public static void ConfigureVideoEndpoints(this WebApplication app)
    {
        app.MapGet("video-fetcher/api/v1/video{id}", GetVideo)
            .WithName("GetVideo")
            .Produces<YouTubeVideoDto>(200)
            .Produces(400);
    }

    private async static Task<IResult> GetVideo(IVideoService _videoService, IMapper _mapper, string videoId)
    {
        Console.WriteLine("GetVideo endpoint executed.");
        APIResponse response = new APIResponse();

        try
        {
            Video video = await _videoService.GetVideoByIdAsync(videoId);
            YouTubeVideoDto videoDto = _mapper.Map<YouTubeVideoDto>(video);
            response.Result = videoDto;
            
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }
        catch (VideoNotFoundException ex)
        {
            response.StatusCode = HttpStatusCode.NotFound;
            response.ErrorMessages.Add(ex.Message);
            return Results.NotFound(response);
        }
        catch (VideoAccessForbiddenException ex)
        {
            response.StatusCode = HttpStatusCode.Forbidden;
            response.ErrorMessages.Add(ex.Message);
            return Results.Forbid();
        }
        catch (VideoBadRequestException ex)
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.ErrorMessages.Add(ex.Message);
            return Results.BadRequest(response);
        }
        catch (GoogleApiException ex)
        {
            response.StatusCode = HttpStatusCode.InternalServerError;
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            response.StatusCode = HttpStatusCode.InternalServerError;
            return Results.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
