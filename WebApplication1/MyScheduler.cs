using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class MyScheduler : IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory _jobFactory;
        private JobMeta _jobMetadata;
        private readonly ISchedulerFactory _schedulerFactory;

        public MyScheduler(IJobFactory jobFactory, JobMeta jobMetadata, ISchedulerFactory schedulerFactory)
        {
            _jobFactory = jobFactory;
            _jobMetadata = jobMetadata;
            _schedulerFactory = schedulerFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            int i = 0;
            while (i < 2)
            {
                Scheduler = await _schedulerFactory.GetScheduler();
                Scheduler.JobFactory = _jobFactory;

                if (i == 1)
                {
                    _jobMetadata.JobId = Guid.NewGuid();
                    _jobMetadata.JobName = "Tudo novamente";
                    _jobMetadata.CronExpression = "0/2 * * * * ?";
                }

                IJobDetail job = CreateJob(_jobMetadata);

                ITrigger trigger = CreateTrigger(_jobMetadata);

                await Scheduler.ScheduleJob(job, trigger, cancellationToken);

                await Scheduler.Start(cancellationToken);
                i++;
            }
        }

        private ITrigger CreateTrigger(JobMeta jobMetadata) =>
            TriggerBuilder.Create()
                            .WithIdentity(jobMetadata.JobId.ToString())
                            .WithDescription(jobMetadata.JobName)
                            .WithCronSchedule(jobMetadata.CronExpression)
                            .Build();

        private IJobDetail CreateJob(JobMeta jobMetadata) =>
            JobBuilder.Create(jobMetadata.JobType)
                                .WithIdentity(jobMetadata.JobId.ToString())
                                .WithDescription(jobMetadata.JobName)
                                .UsingJobData("Cron", jobMetadata.CronExpression)
                                .UsingJobData("Nome", jobMetadata.JobName)
                                .Build();

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler.Shutdown();
        }
    }
}
