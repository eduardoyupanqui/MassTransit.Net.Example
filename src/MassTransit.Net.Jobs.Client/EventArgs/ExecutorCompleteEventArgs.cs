using MassTransit.Net.Jobs.Client.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorCompleteEventArgs : System.EventArgs, JobCompleted
    {
        public Guid JobId { get; set; }
        public string OutputJob { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
