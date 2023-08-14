using AutoMapper;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;

namespace CommentsFetchStatusIntegration.Worker.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<FetchInfoChangedEvent, VideoFetchInfo>();
    }
}