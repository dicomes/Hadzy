using AutoMapper;
using CommentsFetchStatusIntegration.Worker.Builders;
using CommentsFetchStatusIntegration.Worker.IntegrationEvents;
using CommentsFetchStatusIntegration.Worker.Models;
using CommentsFetchStatusIntegration.Worker.Services.Interfaces;
using MassTransit;
using IntegrationEventsContracts;

namespace CommentsFetchStatusIntegration.Worker.Consumers;

public class CommentsFetchStatusEventConsumer : IConsumer<ICommentsFetchStatusEvent>
{
    private readonly ILogger<CommentsFetchStatusEventConsumer> _logger;
    private readonly IVideoCommentsStatusService _videoCommentsStatusService;
    private readonly IMapper _mapper;

    public CommentsFetchStatusEventConsumer(ILogger<CommentsFetchStatusEventConsumer> logger, IVideoCommentsStatusService videoCommentsStatusService, IMapper mapper)
    {
        _logger = logger;
        _videoCommentsStatusService = videoCommentsStatusService;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<ICommentsFetchStatusEvent> context)
    {
        CommentsFetchStatusEventBuilder commentsFetchStatusEventBuilder = new CommentsFetchStatusEventBuilder()
            .WithId(context.Message.Id)
            .WithVideoId(context.Message.VideoId)
            .WithPageToken(context.Message.PageToken)
            .WithCommentsFetchedCount(context.Message.CommentsFetchedCount)
            .WithReplyCount(context.Message.ReplyCount)
            .WithIsFetching(context.Message.IsFetching);

        CommentsFetchStatusEvent fetchStatus = commentsFetchStatusEventBuilder.Build();

        _logger.LogInformation("CommentsFetchStatusEventConsumer: Received CommentsFetchStatusEvent. Guid: {Guid}. VideoId: {VideoId}. PageToken: {CommentsCount}.", fetchStatus.Id, fetchStatus.VideoId, fetchStatus.CommentsFetchedCount);

        VideoCommentsStatus videoCommentsStatus;

        // Check if the videoId exists in the database
        bool videoIdExists = await _videoCommentsStatusService.VideoIdExistsAsync(fetchStatus.VideoId);

        // Update or insert the videoCommentsStatus in the database based on videoId existence
        if (videoIdExists)
        {
            videoCommentsStatus = await _videoCommentsStatusService.GetVideoCommentsStatusByVideoIdAsync(fetchStatus.VideoId);
            videoCommentsStatus.TotalCommentsFetched += fetchStatus.CommentsFetchedCount + fetchStatus.ReplyCount;
            await _videoCommentsStatusService.UpdateVideoCommentsStatusAsync(videoCommentsStatus);
        }
        else
        {
            videoCommentsStatus = _mapper.Map<VideoCommentsStatus>(fetchStatus);
            await _videoCommentsStatusService.InsertVideoCommentsStatusAsync(videoCommentsStatus); 
        }

    }
}