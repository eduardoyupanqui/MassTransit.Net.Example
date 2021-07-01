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
            private readonly ILogger<ActualizarInicioJobCommandHandler> _logger;
            public ActualizarInicioJobCommandHandler(
                IMediator mediator,
                ILogger<ActualizarInicioJobCommandHandler> logger)
            {
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            }
            public async Task<bool> Handle(ActualizarInicioJobCommand request, CancellationToken cancellationToken)
            {
                _logger.LogDebug($"Verificando si existe el job a procesar de estado PENDIENTE: El job {request.IdJob}");
                return true;
            }
        }
    }
}
