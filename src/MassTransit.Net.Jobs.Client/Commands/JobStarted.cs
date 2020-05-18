using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobStarted
    {
        public Guid JobId { get; }
        public DateTime FechaInicio { get; }
    }
}
