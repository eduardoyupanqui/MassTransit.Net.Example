using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobFailed
    {
        public Guid JobId { get; }
        public string Mensaje { get; }
        public string StackTrace { get; }
    }
}
