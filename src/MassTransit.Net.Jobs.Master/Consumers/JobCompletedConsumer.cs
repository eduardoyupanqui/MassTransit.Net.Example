using MassTransit.Net.Jobs.Client.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Master.Consumers
{
    public class JobCompletedConsumer : IConsumer<JobCompleted>
    {
        private readonly ILogger _logger;
        public JobCompletedConsumer(ILogger<JobCompletedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<JobCompleted> context)
        {
            _logger.LogInformation($"JobId: {context.Message.JobId} Complete on : {context.Message.FechaFin}");
            await Task.Delay(1000);
        }
    }
}
