using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorFailEventArgs : System.EventArgs
    {
        public string Mensaje { get; set; }
        public string StackTrace { get; set; }
    }
}
