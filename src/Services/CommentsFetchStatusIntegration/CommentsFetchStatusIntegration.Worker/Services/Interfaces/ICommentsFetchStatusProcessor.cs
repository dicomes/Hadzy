using CommentsFetchStatusIntegration.Worker.IntegrationEvents;

namespace CommentsFetchStatusIntegration.Worker.Services.Interfaces;

public interface ICommentsFetchStatusProcessor
{
    Task ProcessVideoCommentsStatusAsync(CommentsFetchStatusEvent fetchStatusEvent);
}