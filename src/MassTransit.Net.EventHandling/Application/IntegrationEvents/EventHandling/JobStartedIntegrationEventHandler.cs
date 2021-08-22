using MassTransit;
using MassTransit.Net.EventHandling.Application.IntegrationEvents.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Context;
using System.Threading;
using MassTransit.Net.EventHandling.Application.Commands.Job;

namespace MassTransit.Net.EventHandling.Application.IntegrationEvents.EventHandling
{
    public class JobStartedIntegrationEventHandler : IConsumer<JobStartedIntegrationEvent>
    {
        private readonly ILogger<JobStartedIntegrationEventHandler> _logger;
        private readonly MediatR.IMediator _mediator;
        public JobStartedIntegrationEventHandler(
            ILogger<JobStartedIntegrationEventHandler> logger,
            MediatR.IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<JobStartedIntegrationEvent> context)
        {
            var @event = context.Message;
            using (LogContext.PushProperty("IntegrationEventContext", $"{context.MessageId}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.MessageId, Program.AppName, @event);

            }
        }
    }
}
