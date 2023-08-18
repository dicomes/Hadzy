using CommentsFetchInfoManager.MinimalApi.Models;

namespace CommentsFetchInfoManager.MinimalApi.Services.Interfaces;

public interface IFetchInfoRepository
{
    Task<VideoFetchInfo> GetByVideoId(string videoId);
    Task Add(VideoFetchInfo videoFetchInfo);
    Task Update(VideoFetchInfo videoFetchInfo);
}