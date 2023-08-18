using AutoMapper;
using CommentsFetchInfoIntegration.Worker.IntegrationEvents;
using CommentsFetchInfoIntegration.Worker.Models;

namespace CommentsFetchInfoIntegration.Worker.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<FetchInfoChangedEvent, VideoFetchInfo>();
    }
}