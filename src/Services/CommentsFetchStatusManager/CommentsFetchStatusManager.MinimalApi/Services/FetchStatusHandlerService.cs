using System.Net;
using AutoMapper;
using CommentsFetchStatus.MinimalApi.Configurations;
using CommentsFetchStatus.MinimalApi.IntegrationEvents;
using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;
using CommentsFetchStatus.MinimalApi.Services.Interfaces;
using FluentValidation;
using IntegrationEventsContracts;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace CommentsFetchStatus.MinimalApi.Services;

public class FetchStatusHandlerService : IFetchStatusHandlerService
{
    private readonly IMongoCollection<VideoFetchInfo> _videoFetchInfo;
    private readonly IMapper _mapper;
    private readonly IEventPublisherService _eventPublisherService;
    private readonly IValidator<FetchInfoDto> _validator;

    public FetchStatusHandlerService(
        IOptions<MongoDbConfig> mongoDbConfig,
        IMapper mapper,
        IEventPublisherService eventPublisherService,
        IValidator<FetchInfoDto> validator)
    {
        var mongoClient = new MongoClient(mongoDbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _videoFetchInfo =
            mongoDatabase.GetCollection<VideoFetchInfo>(mongoDbConfig.Value.VideoFetchInfoCollectionName);
        _mapper = mapper;
        _eventPublisherService = eventPublisherService;
        _validator = validator;
    }

    public async Task<IResult> GetFetchStatusByIdAsync(string videoId)
    {
        if (string.IsNullOrEmpty(videoId))
        {
            var apiResponse = CreateErrorResponse<FetchInfoDto>("Video Id cannot be blank");
            return Results.BadRequest(apiResponse);
        }

        VideoFetchInfo videoFetchInfo = await _videoFetchInfo.Find<VideoFetchInfo>(f => f.VideoId == videoId).FirstOrDefaultAsync();
        if (videoFetchInfo == null)
        {
            var apiResponse = CreateErrorResponse<FetchInfoDto>("Video Id not found");
            return Results.NotFound(apiResponse);
        }

        var fetchInfoDto = _mapper.Map<FetchInfoDto>(videoFetchInfo);
        var response = new APIResponse<FetchInfoDto>
        {
            Result = fetchInfoDto
        };

        return Results.Ok(response);
    }

    public async Task<IResult> PostNewFetchInfo(FetchInfoDto? fetchInfoDto)
    {
        var response = new APIResponse<FetchInfoDto>();

        var validationResult = await ValidateFetchInfo(fetchInfoDto);
        
        if (!validationResult.IsValid)
        {
            response.ErrorMessages = validationResult.ErrorMessages;
            return Results.BadRequest(response);
        }

        VideoFetchInfo oldVideoFetchInfo =
            await _videoFetchInfo.Find<VideoFetchInfo>(f => f.VideoId == fetchInfoDto.VideoId)
                .FirstOrDefaultAsync();

        if (oldVideoFetchInfo == null)
        {
            var newFetchStartedEvent = new FetchStartedEvent(fetchInfoDto.VideoId);
            await _eventPublisherService.PublishEvent<IFetchStartedEvent>(newFetchStartedEvent);
            response.Result = new FetchInfoDto()
            {
                VideoId = fetchInfoDto.VideoId,
                Status = FetchStatus.InProgress.ToString()
            };
            return Results.Accepted(value: response);
        }
        
        if (oldVideoFetchInfo.Status == FetchStatus.InProgress.ToString())
        {
            var newFetchInfoDto = _mapper.Map<FetchInfoDto>(oldVideoFetchInfo);
            response.Result = newFetchInfoDto;
            return Results.Accepted(value: response);
        }
        
        if (oldVideoFetchInfo.Status == FetchStatus.Done.ToString())
        {
            var newFetchStartedEvent = CreateFetchStartedEvent(fetchInfoDto.VideoId, oldVideoFetchInfo);
            await _eventPublisherService.PublishEvent<IFetchStartedEvent>(newFetchStartedEvent);
            response.Result = new FetchInfoDto()
            {
                VideoId = fetchInfoDto.VideoId,
                Status = FetchStatus.InProgress.ToString()
            };
            return Results.Accepted(value: response);
        }
        
        return Results.Accepted(value: response);
    }
    
    private async Task<ValidationResult> ValidateFetchInfo(FetchInfoDto? fetchInfoDto)
    {
        var validationResult = new ValidationResult();

        if (fetchInfoDto == null)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessages.Add("Request body is missing");
            return validationResult;
        }

        var results = await _validator.ValidateAsync(fetchInfoDto);
        
        if (!results.IsValid)
        {
            validationResult.IsValid = false;
            validationResult.ErrorMessages = results.Errors.Select(e => e.ErrorMessage).ToList();
        }

        return validationResult;
    }

    private FetchInfoDto? UpdateFetchInfo(FetchInfoDto? fetchInfoDto, VideoFetchInfo? videoFetchInfo)
    {
        if (videoFetchInfo == null)
        {
            fetchInfoDto.Status = FetchStatus.InProgress.ToString();
            return fetchInfoDto;
        }
        
        return _mapper.Map<FetchInfoDto>(videoFetchInfo);
    }

    private FetchStartedEvent CreateFetchStartedEvent(string videoId, VideoFetchInfo? oldVideoFetchInfo)
    {
        var newFetchStartedEvent = new FetchStartedEvent(videoId)
        {
            PageToken = oldVideoFetchInfo.LastPageToken
        };
        
        return newFetchStartedEvent;
    }

    private class ValidationResult
    {
        public bool IsValid { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
    
    public APIResponse<T> CreateErrorResponse<T>(string errorMessage)
    {
        var response = new APIResponse<T>()
        {
            Result = default
        };

        if (!string.IsNullOrEmpty(errorMessage))
        {
            response.ErrorMessages.Add(errorMessage);
        }

        return response;
    }

}