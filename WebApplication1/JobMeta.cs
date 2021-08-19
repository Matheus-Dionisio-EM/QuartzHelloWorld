using System;

namespace WebApplication1
{
    public class JobMeta
    {
        public Guid JobId { get; set; }

        public Type JobType { get; set; }

        public string JobName { get; set; }

        public string CronExpression { get; set; }

        public JobMeta(Guid jobId, Type jobType, string jobName, string cronExpression)
        {
            JobId = jobId;
            JobType = jobType;
            JobName = jobName;
            CronExpression = cronExpression;
        }
    }
}
