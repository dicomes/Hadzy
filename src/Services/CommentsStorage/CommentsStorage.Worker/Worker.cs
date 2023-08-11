using MassTransit;
using IntegrationEventsContracts;

namespace CommentsStorage.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    private struct FetchEvent
    {
        public string VideoId;
        public string PageToken;
    }
    
    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<FetchEvent> videoList = new List<FetchEvent>();
        // videoList.Add(new FetchEvent {VideoId = "h3bt79JUYak", PageToken = "QURTSl9pMmxMeUZuLXg1bGVPNDJnVzUxVGVjNkJNaVYxR2JYSGp4RmhhcXRTZmJ0aHBPRkY1cmF6V2ZQRzVWOVdYMGt1TGRVYVVTc1JOTQ=="});
        videoList.Add(new FetchEvent {VideoId = "h3bt79JUYak", PageToken = string.Empty});
        // videoList.Add(new FetchEvent {VideoId = "sdasd", PageToken = string.Empty});
        // videoList.Add(new FetchEvent {VideoId = "Q_RxN7FqV8M", PageToken = string.Empty});

        while (!stoppingToken.IsCancellationRequested && videoList.Count != 0)
        {
            _logger.LogInformation("Worker running at: {time}.", DateTimeOffset.Now);
        
            // Create a scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
            await publishEndpoint.Publish<IFetchStartedEvent>(new
            {
                VideoId = videoList[0].VideoId,
                PageToken = videoList[0].PageToken
            });
            _logger.LogInformation("Published CommentsFetchingInitiatedEvent: {VideoID}. PageToken: {PageToken}.", videoList[0].VideoId, videoList[0].PageToken);

            videoList.Remove(videoList[0]);
        
            await Task.Delay(1000, stoppingToken);
        }
    }

}
