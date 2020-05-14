using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorFailEventArgs
    {
        public string Mensaje { get; set; }
        public string StackTrace { get; set; }
    }
}
