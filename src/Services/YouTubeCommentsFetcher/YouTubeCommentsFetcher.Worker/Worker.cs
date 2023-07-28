using YouTubeCommentsFetcher.Worker.Models;
using YouTubeCommentsFetcher.Worker.Services;

namespace YouTubeCommentsFetcher.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ICommentsService _commentsService;

    public Worker(ILogger<Worker> logger, ICommentsService commentsService)
    {
        _logger = logger;
        _commentsService = commentsService;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            // Here code to consume the videoId from RabbitMq via MassTransit.
            // After videoId is received a fetching process starts below
            
            var batch = await _commentsService.GetCommentBatchByVideoIdAsync(new FetchSettings(){VideoId = "WAuEByCltMA", MaxResults = 100, Properties = "snippet"});
            
            _logger.LogInformation("Fetched comments for video {videoId}. Nr comments fetched: {commentsFetched}", batch.VideoId, batch.YouTubeCommentsList.Count);
            // After the batch is ready publish to a RabbitMq queue via MassTransit
            
            await Task.Delay(100, stoppingToken);
        }
    }
}
