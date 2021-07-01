using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Application.IntegrationEvents.Events
{
    public class JobEnqueueIntegrationEvent
    {
        public Guid UsuarioRegistro { get; set; }
        public Guid IdJob { get; set; }

        public Guid IdAplicacion { get; set; }

        public string RegistroAsociado { get; set; }
        public string InputJob { get; set; }
        public string HostName { get; set; }

        public string CodigoTipoJob { get; set; }
        public string NombreJob { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
