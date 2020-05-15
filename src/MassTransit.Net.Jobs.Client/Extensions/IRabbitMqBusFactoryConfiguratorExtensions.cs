using MassTransit.Net.Jobs.Client.Consumers;
using MassTransit.RabbitMqTransport;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client.Extensions
{
    public static class IRabbitMqBusFactoryConfiguratorExtensions
    {
        public static void ReceivedJobEndpoint<T>(this IRabbitMqBusFactoryConfigurator cfg, IServiceProvider provider) 
            where T : BaseExecutor
        {
            cfg.ReceiveEndpoint(typeof(T).Name.ToUnderscoreCase(), ep =>
            {
                ep.Consumer<JobConsumer<T>>(provider);
            });
        }
    }
}
