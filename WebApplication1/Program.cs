using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using WebApplication1.Job;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<IJobFactory, JobFactory>();
                        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
                        services.AddSingleton(new JobMeta(Guid.NewGuid(), typeof(HelloWorld),
                            " Hello World Job", "0/1 * * * * ?"));
                        services.AddSingleton<HelloWorld>();


                        services.AddHostedService<MyScheduler>();
                    }
                );
    }
}
