namespace YouTubeCommentsFetcher.Worker.Contracts;

public interface IEventPublisher
{
    Task PublishEvent<TEvent>(TEvent eventObject);
}