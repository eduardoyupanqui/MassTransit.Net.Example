using MassTransit.Net.Jobs.Client.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Master.Consumers
{
    public class JobFailedConsumer : IConsumer<JobFailed>
    {
        private readonly ILogger _logger;
        public JobFailedConsumer(ILogger<JobFailedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<JobFailed> context)
        {
            _logger.LogInformation($"JobId: {context.Message.JobId} Failed on : {context.Message.Mensaje} {context.Message.StackTrace}");
            await Task.Delay(1000);
        }
    }
}
