using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorCompleteEventArgs : System.EventArgs
    {
        public DateTime FechaFin { get; set; }
    }
}
