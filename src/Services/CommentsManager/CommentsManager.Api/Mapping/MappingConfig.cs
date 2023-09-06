using AutoMapper;
using CommentsManager.Api.DTO;
using CommentsManager.Api.Models;

namespace CommentsManager.Api.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Comment, CommentResponse>();
    }
}