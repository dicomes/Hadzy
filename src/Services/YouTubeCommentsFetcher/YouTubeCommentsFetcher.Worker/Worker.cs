using MassTransit;
using MassTransit.Util;

namespace YouTubeCommentsFetcher.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IBusControl _busControl;

        public Worker(ILogger<Worker> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("YouTubeCommentsFetcher started. Waiting for CommentsFetchReceivedEvent...");

            // Starting the bus control initiates the MassTransit service and begins listening for messages.
            // When a message of type IFetchStartedEvent is received, MassTransit will automatically 
            // instantiate the associated consumer (FetchStartedEventConsumer) and invoke its Consume method 
            // to process the message.
            await _busControl.StartAsync(stoppingToken);

            // Wait until the task is cancelled.
            await TaskUtil.Completed;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }
    }

}

