using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.DependencyInjection
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string SomeValue { get; set; }

        public DateTime Timestamp { get; set; }
        public short StatusCode { get; set; }
        public string StatusText { get; set; }
    }
}
