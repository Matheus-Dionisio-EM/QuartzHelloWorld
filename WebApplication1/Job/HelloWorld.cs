using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Job
{
    public class HelloWorld : IJob
    {
        private readonly ILogger<HelloWorld> _logger;

        public HelloWorld(ILogger<HelloWorld> logger)
        {
            _logger = logger;
        }


        public Task Execute(IJobExecutionContext context)
        {
            var message = $"Job: Nome: {context.JobDetail.JobDataMap["Nome"]} Id: {context.JobDetail.Key} Cron: {context.JobDetail.JobDataMap["Cron"]}";
            _logger.LogInformation(message);

            return Task.CompletedTask;
        }
    }
}
