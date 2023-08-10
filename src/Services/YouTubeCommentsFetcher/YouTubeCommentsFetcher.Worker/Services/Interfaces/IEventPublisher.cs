namespace YouTubeCommentsFetcher.Worker.Services.Interfaces;

public interface IEventPublisher
{
    Task PublishEvent<TEvent>(TEvent eventObject);
}