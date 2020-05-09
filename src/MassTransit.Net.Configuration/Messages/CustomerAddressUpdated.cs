using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Configuration.Messages
{
    public interface CustomerAddressUpdated
    {
        string CustomerId { get; }
    }
}
