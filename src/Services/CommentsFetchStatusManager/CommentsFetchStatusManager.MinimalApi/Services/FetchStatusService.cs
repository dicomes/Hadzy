using AutoMapper;
using CommentsFetchStatus.MinimalApi.Configurations;
using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;
using CommentsFetchStatus.MinimalApi.Services.Interfaces;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace CommentsFetchStatus.MinimalApi.Services;

public class FetchStatusService : IFetchStatusService
{
    private readonly IMongoCollection<VideoFetchStatus> _fetchStatus;
    private readonly IMapper _mapper;

    public FetchStatusService(
        IOptions<MongoDbConfig> mongoDbConfig,
        IMapper mapper)
    {
        var mongoClient = new MongoClient(mongoDbConfig.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbConfig.Value.DatabaseName);
        _fetchStatus = mongoDatabase.GetCollection<VideoFetchStatus>(mongoDbConfig.Value.CommentsFetchStatusCollectionName);
        _mapper = mapper;
    }

    public async Task<IResult> GetStatusByIdAsync(string videoId)
    {
        VideoFetchStatus status =  await _fetchStatus.Find<VideoFetchStatus>(f => f.VideoId == videoId).FirstOrDefaultAsync();
        
        if (status == null)
        {
            return Results.NotFound();
        }

        CommentsFetchStatusDto commentsFetchStatusDto = _mapper.Map<CommentsFetchStatusDto>(status);
        
        var response = new APIResponse<CommentsFetchStatusDto>
        {
            Result = commentsFetchStatusDto
        };

        return Results.Ok(response);
    }
    
}