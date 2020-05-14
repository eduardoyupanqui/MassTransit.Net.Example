using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Net.Jobs.Client
{
    public abstract class BaseExecutor : IExecutor
    {
        public event EventHandler<ExecutorStartEventArgs> ProcessStarted;
        public event EventHandler<ExecutorTaskEventArgs> StatusTarea;
        public event EventHandler<ExecutorCompleteEventArgs> ProcessCompleted;
        public event EventHandler<ExecutorFailEventArgs> ProcessFailed;

        protected BaseExecutor()
        {

        }

        public virtual void Execute(JobCommand command)
        {
            NofificarInicio();
            try
            {
                EjecutarJob(command);

            }
            catch (Exception ex)
            {
                NofificarError(ex.Message, ex.StackTrace);
                throw;
            }

            NofificarFin();
        }

        public abstract void EjecutarJob(JobCommand command);

        private void NofificarInicio()
        {
            ProcessStarted(this, new ExecutorStartEventArgs()
            {
                FechaInicio = DateTime.Now
            });
        }

        protected void NofificarProgreso(int orden, string mensaje)
        {
            StatusTarea(this, new ExecutorTaskEventArgs()
            {
                Orden = orden,
                Mensaje = mensaje
            });
        }

        private void NofificarError(string message, string stackTrace)
        {
            ProcessFailed(this, new ExecutorFailEventArgs()
            {
                Mensaje = message,
                StackTrace = stackTrace
            });
        }

        private void NofificarFin()
        {
            ProcessCompleted(this, new ExecutorCompleteEventArgs()
            {
                FechaFin = DateTime.Now
            });
        }
    }
}
