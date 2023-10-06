using CommentsStorage.Worker.Models;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Contracts.Services;

public interface IIntegrationService
{
    Task AddComments(List<IYouTubeCommentDto> youTubeCommentDto);
    Task AddVideoByStartedEvent(IFetchStartedEvent fetchStarted);
    Task UpdateVideoByCompletedEvent(IFetchCompletedEvent fetchCompleted);
}