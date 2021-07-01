using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Application.ViewModels
{
    public class JobResponse
    {
        public Guid IdJob { get; set; }

        public Guid IdTipoJob { get; set; }
    }
}
