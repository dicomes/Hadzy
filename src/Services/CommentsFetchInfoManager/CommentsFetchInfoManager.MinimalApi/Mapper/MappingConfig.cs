using AutoMapper;
using CommentsFetchInfoManager.MinimalApi.Models;
using CommentsFetchInfoManager.MinimalApi.Models.DTO;

namespace CommentsFetchInfoManager.MinimalApi.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<VideoFetchInfo, FetchInfoDto>().ReverseMap();
    }
}