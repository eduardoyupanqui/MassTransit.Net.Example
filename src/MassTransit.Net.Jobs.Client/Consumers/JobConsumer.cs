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
    public class JobConsumer : IConsumer<JobCommand>
    {
        private readonly ILogger<JobConsumer> _logger;
        private readonly IExecutor _executor;

        private Guid JobId;
        public JobConsumer(ILogger<JobConsumer> logger, IExecutor executor)
        {
            _logger = logger;
            _executor = executor;

            _executor.ProcessStarted += OnProcessStarted;
            _executor.StatusTarea += OnStatusTarea;
            _executor.ProcessCompleted += OnProcessCompleted;
            _executor.ProcessFailed += OnProcessFailed;
        }

        

        public Task Consume(ConsumeContext<JobCommand> context)
        {
            _logger.LogInformation($"JobId: {context.Message.JobId} , InputJob: {context.Message.JobInput}");
            JobId = context.Message.JobId;
            _executor.Execute(context.Message);
            return Task.CompletedTask;
        }

        private void OnProcessStarted(object sender, ExecutorStartEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Started on : {e.FechaInicio}");
            //Comunicar al Master el inicio del job
        }

        private void OnStatusTarea(object sender, ExecutorTaskEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Execute Tarea {e.Orden} : {e.Mensaje}");
            //Comunicar al Master el progreso de las tareas
        }
        private void OnProcessCompleted(object sender, ExecutorCompleteEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Complete on : {e.FechaFin}");
            //Comunicar al Master el fin del job
        }

        private void OnProcessFailed(object sender, ExecutorFailEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Failed on : {e.Mensaje} {e.StackTrace}");
            //Comunicar al Master que el job ha fallado
        }
    }
}
