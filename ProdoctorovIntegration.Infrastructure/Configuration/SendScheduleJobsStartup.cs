using ProdoctorovIntegration.Infrastructure.Jobs;
using Quartz;
using Quartz.Impl;

namespace ProdoctorovIntegration.Infrastructure.Configuration;

public static class SendScheduleJobsStartup
{
    public static async Task RunAsync(IServiceProvider service, int hoursInterval)
    {
        var factory = new StdSchedulerFactory();
        var scheduler = await factory.GetScheduler();
        scheduler.JobFactory = new ScopedJobFactory(service);
        await scheduler.Start();

        await SendScheduleJob(scheduler, hoursInterval);
    }

    private static async Task SendScheduleJob(IScheduler scheduler, int hoursInterval)
    {
        var job = JobBuilder.Create<SendScheduleJob>().Build();
        var trigger = TriggerBuilder.Create()
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 0))
            .WithSimpleSchedule(x => x.WithIntervalInHours(hoursInterval)
                .RepeatForever())
            .Build();
        await scheduler.ScheduleJob(job, trigger);
    }
}