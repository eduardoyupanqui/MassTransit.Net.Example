using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobTaskCompleted
    {
        public Guid JobId { get; }
        public int Orden { get; }
        public string Mensaje { get; }
        public DateTime FechaEjecucion { get; }
    }
}
