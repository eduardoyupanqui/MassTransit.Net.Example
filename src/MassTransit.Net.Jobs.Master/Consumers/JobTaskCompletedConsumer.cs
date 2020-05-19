using MassTransit.Net.Jobs.Client.Commands;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Master.Consumers
{
    public class JobTaskCompletedConsumer : IConsumer<JobTaskCompleted>
    {
        private readonly ILogger _logger;
        public JobTaskCompletedConsumer(ILogger<JobTaskCompletedConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<JobTaskCompleted> context)
        {
            _logger.LogInformation($"JobId: {context.Message.JobId} Execute Tarea {context.Message.Orden} : {context.Message.Mensaje}");
        }
    }
}
