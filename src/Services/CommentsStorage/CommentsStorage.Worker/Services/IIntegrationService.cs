using CommentsStorage.Worker.Models;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker.Services;

public interface IIntegrationService
{
    Task AddComments(List<IYouTubeCommentDto> youTubeCommentDto);
}