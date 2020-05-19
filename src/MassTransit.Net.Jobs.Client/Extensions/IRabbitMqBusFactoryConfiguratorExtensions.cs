using MassTransit.Net.Jobs.Client.Commands;
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
            EndpointConvention.Map<JobStarted>(new Uri($"queue:{typeof(JobStarted).Name.ToUnderscoreCase()}"));
            EndpointConvention.Map<JobTaskCompleted>(new Uri($"queue:{typeof(JobTaskCompleted).Name.ToUnderscoreCase()}"));
            EndpointConvention.Map<JobCompleted>(new Uri($"queue:{typeof(JobCompleted).Name.ToUnderscoreCase()}"));
            EndpointConvention.Map<JobFailed>(new Uri($"queue:{typeof(JobFailed).Name.ToUnderscoreCase()}"));
            cfg.ReceiveEndpoint(typeof(T).Name.ToUnderscoreCase(), ep =>
            {
                ep.Consumer<JobConsumer<T>>(provider);
            });
        }
    }
}
