using AutoMapper;
using CommentsFetchStatus.MinimalApi.Models;
using CommentsFetchStatus.MinimalApi.Models.DTO;

namespace CommentsFetchStatus.MinimalApi.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VideoFetchInfo, FetchInfoDto>().ReverseMap();
    }
}