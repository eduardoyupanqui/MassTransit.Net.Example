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

        private Guid JobId;
        private ConsumeContext<JobCommand> _context;
        public JobConsumer(ILogger<T> logger, T executor)
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
            _context = context;
            JobId = context.Message.JobId;
            _executor.Execute(context.Message);
            return Task.CompletedTask;
        }

        private void OnProcessStarted(object sender, ExecutorStartEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Started on : {e.FechaInicio}");
            //Comunicar al Master el inicio del job
            if (_context != null)
            {
                _context.Send<JobStarted>(new
                {
                    JobId = this.JobId,
                    FechaInicio = e.FechaInicio
                });
            }
        }

        private void OnStatusTarea(object sender, ExecutorTaskEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Execute Tarea {e.Orden} : {e.Mensaje}");
            //Comunicar al Master el progreso de las tareas
            if (_context != null)
            {
                _context.Send<JobTaskCompleted>(new
                {
                    JobId = this.JobId,
                    Orden = e.Orden,
                    Mensaje = e.Mensaje,
                    FechaEjecucion = DateTime.Now
                });
            }
        }
        private void OnProcessCompleted(object sender, ExecutorCompleteEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Complete on : {e.FechaFin}");
            //Comunicar al Master el fin del job
            if (_context != null)
            {
                _context.Send<JobCompleted>(new
                {
                    JobId = this.JobId,
                    FechaFin = DateTime.Now
                });
            }
        }

        private void OnProcessFailed(object sender, ExecutorFailEventArgs e)
        {
            _logger.LogInformation($"JobId: {this.JobId} Failed on : {e.Mensaje} {e.StackTrace}");
            //Comunicar al Master que el job ha fallado
            if (_context != null)
            {
                _context.Send<JobFailed>(new
                {
                    JobId = this.JobId,
                    Mensaje = e.Mensaje,
                    StackTrace = e.StackTrace,
                });
            }
        }
    }
}
