using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.DependencyInjection.MessageContracts
{
    public interface CheckOrderStatus
    {
        string OrderId { get; }
    }

    public interface OrderStatusResult
    {
        string OrderId { get; }
        DateTime Timestamp { get; }
        short StatusCode { get; }
        string StatusText { get; }
    }
}
