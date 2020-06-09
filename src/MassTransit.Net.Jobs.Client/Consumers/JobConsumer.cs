using MassTransit;
using MassTransit.Net.Jobs.Client;
using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client.Consumers
{
    public class JobConsumer<T> : IConsumer<JobCommand> where T : IExecutor
    {
        private readonly ILogger _logger;
        private readonly T _executor;

        public JobConsumer(ILogger<T> logger, T executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public Task Consume(ConsumeContext<JobCommand> context)
        {
            _executor.ProcessStarted += (sender, e) => Task.Run(() => _logger.LogInformation($"JobId: {e.JobId} Started on : {e.FechaInicio}"));
            _executor.StatusTarea += (sender, e) => Task.Run(()=> _logger.LogInformation($"JobId: {e.JobId} Execute Tarea {e.Orden} : {e.Mensaje}"));
            _executor.ProcessCompleted += (sender, e) => Task.Run(() => _logger.LogInformation($"JobId: {e.JobId} Complete on : {e.FechaFin}"));
            _executor.ProcessFailed += (sender, e) => Task.Run(() => _logger.LogInformation($"JobId: {e.JobId} Failed on : {e.Mensaje} {e.StackTrace}"));
            
            _executor.ProcessStarted += (sender, e) => context.Send<JobStarted>(e);
            _executor.StatusTarea += (sender, e) => context.Send<JobTaskCompleted>(e);
            _executor.ProcessCompleted += (sender, e) => context.Send<JobCompleted>(e);
            _executor.ProcessFailed += (sender, e) => context.Send<JobFailed>(e);

            _logger.LogInformation($"JobId: {context.Message.JobId} , InputJob: {context.Message.JobInput}");
            //_context = context;
            //JobId = context.Message.JobId;
            return _executor.Execute(context.Message);
        }
    }
}
