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

            var batch = await _commentsService.GetCommentsByVideoIdAsync("4V1RjmLOvmc", 100);
            foreach (var item in batch.YouTubeCommentsList)
            {
                Console.WriteLine(item.TextDisplay);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
