using AutoMapper;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;

namespace CommentsFetchStatusIntegration.Worker.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<FetchStatusChangedEvent, FetchStatus>()
            .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
            .ForMember(dest => dest.TotalCommentsFetched, opt => opt.MapFrom(src => src.CommentsFetchedCount))
            .ForMember(dest => dest.IsFetching, opt => opt.MapFrom(src => src.IsFetching));
    }
}