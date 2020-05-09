using MassTransit.Net.Configuration.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Configuration.Observers
{
    public class CustomerAddressUpdatedObserver : IObserver<ConsumeContext<CustomerAddressUpdated>>
    {
        public void OnNext(ConsumeContext<CustomerAddressUpdated> context)
        {
            Console.WriteLine("Customer address was updated: {0}", context.Message.CustomerId);
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}
