using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobEvent
    {
        public Guid JobId { get; }
    }
}
