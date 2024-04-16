using Microsoft.Extensions.Logging;
using ProdoctorovIntegration.Application.Interfaces;
using Quartz;

namespace ProdoctorovIntegration.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class SendScheduleJob : IJob
{
    private readonly ISendScheduleService _sendScheduleService;
    private readonly ILogger<SendScheduleJob> _logger;
    private readonly IScheduleService _scheduleService;

    public SendScheduleJob(ISendScheduleService sendScheduleService, ILogger<SendScheduleJob> logger, IScheduleService scheduleService)
    {
        _sendScheduleService = sendScheduleService;
        _logger = logger;
        _scheduleService = scheduleService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Job {JobName} has started", nameof(SendScheduleJob));
        try
        {
            var events = await _scheduleService.GetScheduleAsync();
            await _sendScheduleService.SendScheduleAsync(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while executing job {JobName}"
                , nameof(SendScheduleJob));
        }
        _logger.LogInformation("Job {JobName} has finished", nameof(SendScheduleJob));
    }
}