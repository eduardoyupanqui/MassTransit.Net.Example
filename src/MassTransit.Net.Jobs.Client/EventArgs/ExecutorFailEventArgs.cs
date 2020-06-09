using MassTransit.Net.Jobs.Client.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.EventArgs
{
    public class ExecutorFailEventArgs : System.EventArgs, JobFailed
    {
        public Guid JobId { get; set; }
        public string Mensaje { get; set; }
        public string StackTrace { get; set; }
    }
}
