using MassTransit.Net.Jobs.Client;
using MassTransit.Net.Jobs.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client2
{
    public sealed class ReplicarVersionSolicitud : BaseExecutor
    {
        public ReplicarVersionSolicitud() : base()
        {

        }

        public override void EjecutarJob(JobCommand command)
        {
            NofificarProgreso(1, "Se hizo tarea 1");
            NofificarProgreso(2, "Se hizo tarea 2");
            NofificarProgreso(3, "Se hizo tarea 3");
            NofificarProgreso(4, "Se hizo tarea 4");
            NofificarProgreso(5, "Se hizo tarea 5");
        }
    }
}
