using AutoMapper;
using Google.Apis.YouTube.v3.Data;
using YouTubeCommentsFetcher.Worker.IntegrationEvents;
using YouTubeCommentsFetcher.Worker.Models.DTO;

namespace YouTubeCommentsFetcher.Worker.Services.Transformer;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        // Map the Google API's Comment object to YouTubeCommentDto
        CreateMap<CommentThreadSnippet, YouTubeCommentDto>()
            .ForMember(dest => dest.Etag, opt => opt.MapFrom(src => src.TopLevelComment.ETag))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TopLevelComment.Id))
            .ForMember(dest => dest.AuthorDisplayName, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.AuthorDisplayName))
            .ForMember(dest => dest.AuthorProfileImageUrl, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.AuthorProfileImageUrl))
            .ForMember(dest => dest.AuthorChannelUrl, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.AuthorChannelUrl))
            .ForMember(dest => dest.AuthorChannelId, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.AuthorChannelId.Value))
            .ForMember(dest => dest.ChannelId, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.ChannelId))
            .ForMember(dest => dest.VideoId, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.VideoId))
            .ForMember(dest => dest.TextDisplay, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.TextDisplay))
            .ForMember(dest => dest.TextOriginal, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.TextOriginal))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.ParentId))
            .ForMember(dest => dest.CanRate, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.CanRate))
            .ForMember(dest => dest.ViewerRating, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.ViewerRating))
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.LikeCount))
            .ForMember(dest => dest.ModerationStatus, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.ModerationStatus))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.PublishedAtDateTimeOffset))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.TopLevelComment.Snippet.UpdatedAtDateTimeOffset))
            .ForMember(dest => dest.TotalReplyCount, opt => opt.MapFrom(src => src.TotalReplyCount));
        
    }
}
    

