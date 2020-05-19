using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
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

        public virtual async Task Execute(JobCommand command)
        {
            await NofificarInicio();
            try
            {
                await EjecutarJob(command);

            }
            catch (Exception ex)
            {
                await NofificarError(ex.Message, ex.StackTrace);
                throw;
            }

            await NofificarFin();
        }

        public abstract Task EjecutarJob(JobCommand command);

        private async Task NofificarInicio()
        {
            await (ProcessStarted?.Invoke(this, new ExecutorStartEventArgs()
            {
                FechaInicio = DateTime.Now
            }) ?? Task.CompletedTask).ConfigureAwait(false);
        }

        protected async Task NofificarProgreso(int orden, string mensaje)
        {
            await (StatusTarea?.Invoke(this, new ExecutorTaskEventArgs()
            {
                Orden = orden,
                Mensaje = mensaje
            }) ?? Task.CompletedTask).ConfigureAwait(false);
        }

        private async Task NofificarError(string message, string stackTrace)
        {
            await (ProcessFailed?.Invoke(this, new ExecutorFailEventArgs()
            {
                Mensaje = message,
                StackTrace = stackTrace
            }) ?? Task.CompletedTask).ConfigureAwait(false);
        }

        private async Task NofificarFin()
        {
            await (ProcessCompleted?.Invoke(this, new ExecutorCompleteEventArgs()
            {
                FechaFin = DateTime.Now
            }) ?? Task.CompletedTask).ConfigureAwait(false);
        }
    }
}
