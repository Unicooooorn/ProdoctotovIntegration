using Microsoft.Extensions.Logging;
using ProdoctorovIntegration.Application.Requests.Schedule;
using ProdoctorovIntegration.Application.Services;
using Quartz;

namespace ProdoctorovIntegration.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class SendScheduleJob : IJob
{
    private readonly ISendScheduleService _sendScheduleService;
    private readonly ILogger<SendScheduleJob> _logger;
    private readonly IScopedRequestExecutor _scopedRequestExecutor;

    public SendScheduleJob(ISendScheduleService sendScheduleService, ILogger<SendScheduleJob> logger, IScopedRequestExecutor scopedRequestExecutor)
    {
        _sendScheduleService = sendScheduleService;
        _logger = logger;
        _scopedRequestExecutor = scopedRequestExecutor;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Job {JobName} has started", nameof(SendScheduleJob));
        try
        {
            var events = await _scopedRequestExecutor.Execute(new GetScheduleRequest());
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