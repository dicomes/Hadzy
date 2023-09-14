using CommentsStorage.Worker.Models;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Contracts.Services;

public interface IIntegrationService
{
    Task AddComments(List<IYouTubeCommentDto> youTubeCommentDto);
    Task HandleVideo(ICommentThreadListCompletedEvent commentThreadEvent);
}