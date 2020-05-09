using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Configuration
{
    public interface SubmitOrder
    {
        string CustomerType { get; }
        Guid TransactionId { get; }
        // ...
    }
    public class OrderConsumer : IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {

        }
    }
}
