using AutoMapper;
using CommentsStorage.Worker.Models;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<IYouTubeCommentDto, Comment>();
    }
}