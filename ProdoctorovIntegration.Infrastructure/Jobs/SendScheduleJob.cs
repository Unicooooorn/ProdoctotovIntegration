using MediatR;
using Microsoft.Extensions.Logging;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Application.Services;
using Quartz;

namespace ProdoctorovIntegration.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class SendScheduleJob : IJob
{
    private readonly IMediator _mediator;
    private readonly ISendScheduleService _sendScheduleService;
    private readonly ILogger<SendScheduleJob> _logger;

    public SendScheduleJob(IMediator mediator, ISendScheduleService sendScheduleService, ILogger<SendScheduleJob> logger)
    {
        _mediator = mediator;
        _sendScheduleService = sendScheduleService;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Job {JobName} has started", nameof(SendScheduleJob));
        try
        {
            var events = await _mediator.Send(new GetScheduleRequest());
            await _sendScheduleService.SendScheduleAsync(events, new CancellationToken());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while executing job {JobName}"
                , nameof(SendScheduleJob));
        }
        _logger.LogInformation("Job {JobName} has finished", nameof(SendScheduleJob));
    }
}