using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client
{    
    public abstract class BaseExecutor : IExecutor
    {
        public event AsyncEventHandler<ExecutorStartEventArgs> ProcessStarted;
        public event AsyncEventHandler<ExecutorTaskEventArgs> StatusTarea;
        public event AsyncEventHandler<ExecutorCompleteEventArgs> ProcessCompleted;
        public event AsyncEventHandler<ExecutorFailEventArgs> ProcessFailed;

        protected BaseExecutor()
        {

        }

        public virtual async Task<JobResult> Execute(JobCommand command)
        {
            await NofificarInicio();
            try
            {
                var result = await EjecutarJob(command);//.ConfigureAwait(false);
                await NofificarFin(result);
                return result;

            }
            catch (Exception ex)
            {
                await NofificarError(ex.Message, ex.StackTrace);
                throw;
            }
        }

        public abstract Task<JobResult> EjecutarJob(JobCommand command);

        private Task NofificarInicio()
        {
            return (ProcessStarted?.Invoke(this, new ExecutorStartEventArgs()
            {
                FechaInicio = DateTime.Now
            }) ?? Task.CompletedTask);//.ConfigureAwait(false);
        }

        protected Task NofificarProgreso(int orden, string mensaje)
        {
            return (StatusTarea?.Invoke(this, new ExecutorTaskEventArgs()
            {
                Orden = orden,
                Mensaje = mensaje
            }) ?? Task.CompletedTask);//.ConfigureAwait(false);
        }

        private Task NofificarError(string message, string stackTrace)
        {
            return (ProcessFailed?.Invoke(this, new ExecutorFailEventArgs()
            {
                Mensaje = message,
                StackTrace = stackTrace
            }) ?? Task.CompletedTask);//.ConfigureAwait(false);
        }

        private Task NofificarFin(JobResult jobResult)
        {
            return (ProcessCompleted?.Invoke(this, new ExecutorCompleteEventArgs()
            {
                OutputJob = jobResult.OutputJob,
                FechaFin = DateTime.Now
            }) ?? Task.CompletedTask);//.ConfigureAwait(false);
        }
    }
}
