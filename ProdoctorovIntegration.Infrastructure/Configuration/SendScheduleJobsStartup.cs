using ProdoctorovIntegration.Infrastructure.Jobs;
using Quartz;
using Quartz.Impl;

namespace ProdoctorovIntegration.Infrastructure.Configuration;

public static class SendScheduleJobsStartup
{
    public static async Task RunAsync(IServiceProvider service, int minuteInterval)
    {
        var factory = new StdSchedulerFactory();
        var scheduler = await factory.GetScheduler();
        scheduler.JobFactory = new ScopedJobFactory(service);
        await scheduler.Start();

        await SendScheduleJob(scheduler, minuteInterval);
    }

    private static async Task SendScheduleJob(IScheduler scheduler, int minuteInterval)
    {
        var job = JobBuilder.Create<SendScheduleJob>().Build();
        var trigger = TriggerBuilder.Create()
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(minuteInterval)
                .RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}