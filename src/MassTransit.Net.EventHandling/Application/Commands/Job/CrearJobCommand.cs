using MassTransit.Net.EventHandling.Application.IntegrationEvents.Events;
using MassTransit.Net.EventHandling.Application.ViewModels;
using MassTransit.Net.EventHandling.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Application.Commands.Job
{
    public class CrearJobCommand : IRequest<JobResponse>
    {
        public Guid IdAplicacion { get; set; }
        public string CodigoTipoJob { get; set; }

        public string RegistroAsociado { get; set; }//IdRegistroAsociado
        public string InputJob { get; set; }//DataDeEntradaPara ejecutar el Job
        public string HostName { get; set; }

        public string Comentario { get; set; }

        public class CrearJobCommandHandler : IRequestHandler<CrearJobCommand, JobResponse>
        {
            private readonly IMediator _mediator;
            private readonly IPublishEndpoint _publishEndpoint;
            private readonly ILogger<CrearJobCommandHandler> _logger;

            public CrearJobCommandHandler(
                MediatR.IMediator mediator,
                IPublishEndpoint publishEndpoint,
                ILogger<CrearJobCommandHandler> logger)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
            public async Task<JobResponse> Handle(CrearJobCommand objJob, CancellationToken cancellationToken)
            {
                var newGuid = Guid.NewGuid();
                var eventMessage = new JobEnqueueIntegrationEvent
                {
                    IdJob = newGuid,
                    IdAplicacion = objJob.IdAplicacion,
                    RegistroAsociado = objJob.RegistroAsociado,
                    InputJob = objJob.InputJob,
                    CodigoTipoJob = objJob.CodigoTipoJob,
                    NombreJob = $"Job {newGuid}",
                    HostName = objJob.HostName,
                    UsuarioRegistro = Guid.NewGuid(),
                    FechaInicio = DateTime.Now
                };

                //var registerCommand = _mapper.Map<RegisterOrdenCommand>(ordenViewModel);
                _logger.LogInformation("----- Publicando Integracion de evento: {IntegrationEventId} desde {AppName} - ({@IntegrationEvent})", eventMessage.IdJob, Program.AppName, eventMessage);

               await _publishEndpoint.Publish(eventMessage);
                //var sendEndpoint = await _eventBus.GetSendEndpoint(new Uri($"queue:{typeof(JobCommand).Name.ToUnderscoreCase().ToConcatHost(eventMessage.HostName)}"));
                //await sendEndpoint.Send<JobCommand>(eventMessage);
                _logger.LogInformation($"Se registro evento satisfactoriamente {eventMessage.IdJob}");

                return new JobResponse();
            }
        }
    }
}
