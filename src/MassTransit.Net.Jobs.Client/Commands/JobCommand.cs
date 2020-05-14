using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client.Commands
{
    public interface JobCommand
    {
        public Guid JobId { get; }
        public string CodigoJob { get; }
        public string JobInput { get; }
    }
}
