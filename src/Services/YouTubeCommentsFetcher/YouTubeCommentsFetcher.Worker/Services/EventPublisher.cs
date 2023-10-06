using System.Threading.Tasks;
using MassTransit;
using YouTubeCommentsFetcher.Worker.Contracts;

namespace YouTubeCommentsFetcher.Worker.Services
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(IPublishEndpoint publishEndpoint, ILogger<EventPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PublishEvent<TEvent>(TEvent eventObject)
        {
            await _publishEndpoint.Publish(eventObject);
            LogEvent(eventObject);
        }

        private void LogEvent<TEvent>(TEvent eventObject)
        {
            _logger.LogInformation("{Source}: Event of type {EventType} successfully published. {EventData}", GetType().Name, typeof(TEvent).Name, eventObject);
        }
    }
}