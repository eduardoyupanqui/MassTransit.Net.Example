using MassTransit.Net.Quartz.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Quartz.Consumers
{
    public class NotificationConsumer : IConsumer<SendNotification>
    {
        private readonly ILogger<NotificationConsumer> _logger;
        public NotificationConsumer(ILogger<NotificationConsumer> logger)
        {
            _logger = logger;
        }
        public Task Consume(ConsumeContext<SendNotification> context)
        {
            _logger.LogInformation($"Email: {context.Message.EmailAddress} , Body {context.Message.Body}");
            return Task.CompletedTask;
        }
    }
}
