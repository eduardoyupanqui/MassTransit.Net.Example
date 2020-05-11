using MassTransit.Net.Quartz.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Quartz.Consumers
{
    public class ScheduleNotificationConsumer : IConsumer<ScheduleNotification>
    {
        private readonly ILogger<ScheduleNotificationConsumer> _logger;
        public ScheduleNotificationConsumer(ILogger<ScheduleNotificationConsumer> logger)
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<ScheduleNotification> context)
        {
            _logger.LogInformation($"Email: {context.Message.EmailAddress} , Body {context.Message.Body}");
            await context.ScheduleSend<SendNotification>(new Uri("rabbitmq://localhost/schedule_test_queue"),
                context.Message.DeliveryTime,
                new
                {
                    EmailAddress = context.Message.EmailAddress,
                    Body = context.Message.Body
                });
        }
    }
}
