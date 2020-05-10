using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.DependencyInjection
{
    public interface OrderAccepted
    {
        Guid OrderId { get; }
        string SomeValue { get; }
    }
}
