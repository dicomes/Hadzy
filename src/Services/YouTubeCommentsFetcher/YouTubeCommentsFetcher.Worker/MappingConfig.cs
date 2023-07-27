using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Comment, YouTubeComment>()
            .ForMember(dest => dest.Etag, opt => opt.MapFrom(src => src.ETag))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AuthorDisplayName, opt => opt.MapFrom(src => src.Snippet.AuthorDisplayName))
            .ForMember(dest => dest.AuthorProfileImageUrl, opt => opt.MapFrom(src => src.Snippet.AuthorProfileImageUrl))
            .ForMember(dest => dest.AuthorChannelUrl, opt => opt.MapFrom(src => src.Snippet.AuthorChannelUrl))
            .ForMember(dest => dest.AuthorChannelId, opt => opt.MapFrom(src => src.Snippet.AuthorChannelId.Value))
            .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.Snippet.ChannelId))
            .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.Snippet.VideoId))
            .ForMember(dest => dest.TextDisplay, opt => opt.MapFrom(src => src.Snippet.TextDisplay))
            .ForMember(dest => dest.TextOriginal, opt => opt.MapFrom(src => src.Snippet.TextOriginal))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.Snippet.ParentId))
            .ForMember(dest => dest.CanRate, opt => opt.MapFrom(src => src.Snippet.CanRate))
            .ForMember(dest => dest.ViewerRating, opt => opt.MapFrom(src => src.Snippet.ViewerRating))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.Snippet.LikeCount))
            .ForMember(dest => dest.ModerationStatus, opt => opt.MapFrom(src => src.Snippet.ModerationStatus))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.Snippet.PublishedAt))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.Snippet.UpdatedAt));
    }
}
