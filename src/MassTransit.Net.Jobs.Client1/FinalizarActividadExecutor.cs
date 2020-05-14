using MassTransit.Net.Jobs.Client;
using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client1
{
    public class FinalizarActividadExecutor : IExecutor
    {
        public event EventHandler<ExecutorStartEventArgs> ProcessStarted;
        public event EventHandler<ExecutorTaskEventArgs> StatusTarea;
        public event EventHandler<ExecutorCompleteEventArgs> ProcessCompleted;
        public FinalizarActividadExecutor()
        {

        }

        public void Execute(JobCommand command)
        {
            NofificarInicio();

            NofificarProgreso(1, "Se hizo tarea 1");
            NofificarProgreso(2, "Se hizo tarea 2");
            NofificarProgreso(3, "Se hizo tarea 3");
            NofificarProgreso(4, "Se hizo tarea 4");
            NofificarProgreso(5, "Se hizo tarea 5");

            NofificarFin();
        }

        private void NofificarInicio() {
            ProcessStarted(this, new ExecutorStartEventArgs()
            {
                FechaInicio = DateTime.Now
            });
        }

        private void NofificarProgreso(int orden, string mensaje)
        {
            StatusTarea(this, new ExecutorTaskEventArgs()
            {
                Orden = orden,
                Mensaje = mensaje
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
