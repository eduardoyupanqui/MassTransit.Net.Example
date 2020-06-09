using MassTransit.Net.Jobs.Client.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Master.Consumers
{
    public class JobStartedConsumer : IConsumer<JobStarted>
    {
        private readonly ILogger _logger;
        public JobStartedConsumer(ILogger<JobStartedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<JobStarted> context)
        {
            _logger.LogInformation($"JobId: {context.Message.JobId} Started on : {context.Message.FechaInicio}");
            await Task.Delay(1000);
        }
    }
}
