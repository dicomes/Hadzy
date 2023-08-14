namespace CommentsFetchStatus.MinimalApi.Services.Interfaces;

public interface IEventPublisherService
{
    Task PublishEvent<TEvent>(TEvent eventObject);
}