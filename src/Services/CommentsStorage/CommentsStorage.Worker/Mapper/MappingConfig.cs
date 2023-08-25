using AutoMapper;
using CommentsStorage.Worker.Model;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Mapper;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<IYouTubeCommentDto, Comment>();
    }
}