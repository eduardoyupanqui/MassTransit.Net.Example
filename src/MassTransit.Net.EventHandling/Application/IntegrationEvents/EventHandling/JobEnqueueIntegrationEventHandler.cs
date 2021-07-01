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
    public class JobEnqueueIntegrationEventHandler : IConsumer<JobCommand>
    {
        private readonly ILogger<JobEnqueueIntegrationEventHandler> _logger;
        private readonly MediatR.IMediator _mediator;
        public JobEnqueueIntegrationEventHandler(
            ILogger<JobEnqueueIntegrationEventHandler> logger,
            MediatR.IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<JobCommand> context)
        {
            var @event = context.Message;
            using (LogContext.PushProperty("IntegrationEventContext", $"{context.MessageId}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.MessageId, Program.AppName, @event);

                var command = new ActualizarInicioJobCommand()
                {
                    IdJob = @event.IdJob,
                    FechaInicio = @event.FechaInicio
                };

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    command.GetType().Name,
                    nameof(command.IdJob),
                    command.IdJob,
                    command);

                await _mediator.Send(command, default(CancellationToken));
            }
        }
    }
}
