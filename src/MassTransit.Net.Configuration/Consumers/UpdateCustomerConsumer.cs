using MassTransit.Net.Configuration.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Configuration.Consumers
{
    public class UpdateCustomerConsumer : IConsumer<UpdateCustomerAddress>
    {
        public async Task Consume(ConsumeContext<UpdateCustomerAddress> context)
        {
            await Console.Out.WriteLineAsync($"Updating customer: {context.Message.CustomerId}");

            // update the customer address
        }
    }
}
