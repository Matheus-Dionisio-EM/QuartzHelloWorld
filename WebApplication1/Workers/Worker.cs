using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Job;

namespace WebApplication1.Workers
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();

            IJobDetail job = JobBuilder.Create<HelloWorld>()
                                .WithIdentity("Hello", "group1")
                                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                                    .WithIdentity("trigger", "group1")
                                    .StartNow()
                                    .WithSimpleSchedule(x =>
                                        x.WithIntervalInSeconds(5)
                                        .WithRepeatCount(5))
                                    .Build();

            await scheduler.ScheduleJob(job, trigger);

        }
    }
}
