using MassTransit.Net.Jobs.Client.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorStartEventArgs : System.EventArgs, JobStarted
    {
        public Guid JobId { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
