using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobFailed : JobEvent
    {
        public string Mensaje { get; }
        public string StackTrace { get; }
    }
}
