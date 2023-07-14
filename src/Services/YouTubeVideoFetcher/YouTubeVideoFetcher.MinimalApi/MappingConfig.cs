using AutoMapper;
using YouTubeVideoFetcher.MinimalApi.Models.DTO;
using Google.Apis.YouTube.v3.Data;

namespace YouTubeVideoFetcher.MinimalApi;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Video, YouTubeVideoDto>()
            .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.publishedAt, opt => opt.MapFrom(src => src.Snippet.PublishedAt))
            .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Snippet.Localized.Title))
            .ForMember(dest => dest.channelTitle, opt => opt.MapFrom(src => src.Snippet.ChannelTitle))
            .ForMember(dest => dest.viewCount, opt => opt.MapFrom(src => src.Statistics.ViewCount))
            .ForMember(dest => dest.likeCount, opt => opt.MapFrom(src => src.Statistics.LikeCount))
            .ForMember(dest => dest.commentCount, opt => opt.MapFrom(src => src.Statistics.CommentCount))
            .ForMember(dest => dest.channelId, opt => opt.MapFrom(src => src.Snippet.ChannelId));
    }
}
