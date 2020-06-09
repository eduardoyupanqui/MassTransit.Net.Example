using MassTransit.Net.Jobs.Client.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorTaskEventArgs : System.EventArgs, JobTaskCompleted
    {
        public Guid JobId { get; set; }
        public int Orden { get; set; }
        public string Mensaje { get; set; }

        public DateTime FechaEjecucion { get; set; }
    }
}
