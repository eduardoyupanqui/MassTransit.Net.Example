using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorTaskEventArgs
    {
        public int Orden { get; set; }
        public string Mensaje { get; set; }
    }
}
