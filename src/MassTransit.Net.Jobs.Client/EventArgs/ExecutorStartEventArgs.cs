using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorStartEventArgs : System.EventArgs
    {
        public DateTime FechaInicio { get; set; }
    }
}
