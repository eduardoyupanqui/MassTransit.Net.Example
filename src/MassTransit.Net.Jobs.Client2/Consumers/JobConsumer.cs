using MassTransit;
using MassTransit.Net.Jobs.Client.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client2.Consumers
{
    public class JobConsumer : IConsumer<JobCommand>
    {
        private readonly ILogger<JobConsumer> _logger;
        public JobConsumer(ILogger<JobConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<JobCommand> context)
        {
            _logger.LogInformation($"JobId: {context.Message.JobId} , InputJob: {context.Message.JobInput}");
            return Task.CompletedTask;
        }
    }
}
