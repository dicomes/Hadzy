using MassTransit;
using SharedEventContracts;

namespace CommentsStorage.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        List<string> videoList = new List<string>();
        videoList.Add("_VWC_KGxLhQ");
        videoList.Add("e7qDpgoHGqI");
        videoList.Add("sdasd");
        
        while (!stoppingToken.IsCancellationRequested && videoList.Count != 0)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        
            // Create a scope to retrieve scoped services
            using var scope = _serviceProvider.CreateScope();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
            await publishEndpoint.Publish<IVideoIdMessage>(new
            {
                VideoId = videoList[0]
            });
            _logger.LogInformation("Published VideoId: {VideoID}", videoList[0]);

            videoList.Remove(videoList[0]);
        
            await Task.Delay(1000, stoppingToken);
        }
    }

}
