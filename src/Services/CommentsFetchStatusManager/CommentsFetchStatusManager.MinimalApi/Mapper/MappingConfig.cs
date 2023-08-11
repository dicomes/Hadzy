using AutoMapper;
using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;

namespace CommentsFetchStatus.MinimalApi.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<FetchStatus, CommentsFetchStatusDto>()
            .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.VideoId))
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.TotalCommentsFetched))
            .ForMember(dest => dest.IsFetching, opt => opt.MapFrom(src => src.IsFetching));
    }
}