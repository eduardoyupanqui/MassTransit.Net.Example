using MassTransit.Net.EventHandling.Application.IntegrationEvents.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Application.Commands.Job
{
    public class ActualizarInicioJobCommand : IRequest<bool>
    {
        public Guid IdJob { get; set; }
        public DateTime FechaInicio { get; set; }

        public class ActualizarInicioJobCommandHandler : IRequestHandler<ActualizarInicioJobCommand, bool>
        {
            private readonly IMediator _mediator;
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly ILogger<ActualizarInicioJobCommandHandler> _logger;
            public ActualizarInicioJobCommandHandler(
                IMediator mediator,
                IPublishEndpoint publishEndpoint,
                ILogger<ActualizarInicioJobCommandHandler> logger)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _publishEndpoint = publishEndpoint;
            }
            public async Task<bool> Handle(ActualizarInicioJobCommand request, CancellationToken cancellationToken)
            {
                var newGuid = Guid.NewGuid();
                var eventMessage = new JobStartedIntegrationEvent
                {
                    IdJob = request.IdJob,
                    //IdAplicacion = objJob.IdAplicacion,
                    //RegistroAsociado = objJob.RegistroAsociado,
                    //InputJob = objJob.InputJob,
                    //CodigoTipoJob = objJob.CodigoTipoJob,
                    //NombreJob = $"Job {newGuid}",
                    //HostName = objJob.HostName,
                    UsuarioRegistro = newGuid,
                    FechaInicio = DateTime.Now
                };

                //var registerCommand = _mapper.Map<RegisterOrdenCommand>(ordenViewModel);
                _logger.LogInformation("----- Publicando Integracion de evento: {IntegrationEventId} desde {AppName} - ({@IntegrationEvent})", eventMessage.IdJob, Program.AppName, eventMessage);

                //await _eventBus.Publish(eventMessage);
                await _publishEndpoint.Publish(eventMessage);
                //var sendEndpoint = await _eventBus.GetSendEndpoint(new Uri($"queue:{typeof(JobCommand).Name.ToUnderscoreCase().ToConcatHost(eventMessage.HostName)}"));
                //await sendEndpoint.Send<JobCommand>(eventMessage);
                _logger.LogInformation($"Se registro evento satisfactoriamente {eventMessage.IdJob}");

                return true;
            }
        }
    }
}
