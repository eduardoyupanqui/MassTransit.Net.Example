using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobCompleted
    {
        public Guid JobId { get; }
        public DateTime FechaFin { get; }
    }
}
