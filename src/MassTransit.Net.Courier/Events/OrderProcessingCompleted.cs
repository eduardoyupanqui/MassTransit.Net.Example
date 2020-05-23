using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Courier.Events
{
    public class OrderProcessingCompleted
    {
        Guid TrackingNumber { get; }
        DateTime Timestamp { get; }

        string OrderId { get; }
        string OrderApproval { get; }
    }
}
