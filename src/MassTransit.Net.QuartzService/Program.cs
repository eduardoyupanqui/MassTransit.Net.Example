﻿using MassTransit;
using MassTransit.QuartzIntegration;
using MassTransit.Scheduling;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.QuartzService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var isService = !(Debugger.IsAttached || args.Contains("--console"));

            var builder = CreateHostBuilder(args);

            if (isService)
            {
                await builder.UseWindowsService().Build().RunAsync();
            }
            else
            {
                await builder.RunConsoleAsync();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();

                if (args != null)
                    config.AddCommandLine(args);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));
                services.Configure<QuartzConfig>(hostContext.Configuration.GetSection("quartz"));

                // Service Bus
                services.AddMassTransit(cfg =>
                {
                    cfg.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var options = provider.GetRequiredService<IOptions<AppConfig>>().Value;
                        var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();


                        cfg.Host(options.Host, options.VirtualHost, h =>
                        {
                            h.Username(options.Username);
                            h.Password(options.Password);
                        });

                        cfg.UseJsonSerializer(); // Because we are using json within Quartz for serializer type

                        cfg.ReceiveEndpoint(options.QueueName, endpoint =>
                        {
                          var partitionCount = Environment.ProcessorCount;
                          endpoint.PrefetchCount = (ushort)(partitionCount);
                          var partitioner = endpoint.CreatePartitioner(partitionCount);

                          endpoint.Consumer(() => new ScheduleMessageConsumer(schedulerFactory), x =>
                              x.Message<ScheduleMessage>(m => m.UsePartitioner(partitioner, p => p.Message.CorrelationId)));
                          endpoint.Consumer(() => new CancelScheduledMessageConsumer(schedulerFactory),
                              x => x.Message<CancelScheduledMessage>(m => m.UsePartitioner(partitioner, p => p.Message.TokenId)));
                        });
                    }));
                });

                services.AddHostedService<MassTransitConsoleHostedService>();

                services.AddSingleton(x =>
                {
                    var quartzConfig = x.GetRequiredService<IOptions<QuartzConfig>>().Value
                        .UpdateConnectionString(hostContext.Configuration.GetConnectionString("scheduler-db"))
                        .ToNameValueCollection();
                    return new StdSchedulerFactory(quartzConfig).GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();

                });
            })
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
            });
    }
}
