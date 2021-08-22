using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Application.IntegrationEvents.Events
{
    public class JobStartedIntegrationEvent
    {
        public Guid UsuarioRegistro { get; set; }
        public Guid IdJob { get; set; }

        public Guid IdAplicacion { get; set; }
        public string HostName { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
