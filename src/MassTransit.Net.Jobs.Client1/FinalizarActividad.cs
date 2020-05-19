using MassTransit.Net.Jobs.Client;
using MassTransit.Net.Jobs.Client.Commands;
using MassTransit.Net.Jobs.Client.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Client1
{
    public sealed class FinalizarActividad : BaseExecutor
    {
        public FinalizarActividad() : base()
        {

        }

        public override async Task EjecutarJob(JobCommand command)
        {
            await NofificarProgreso(1, "Se hizo tarea 1");
            await NofificarProgreso(2, "Se hizo tarea 2");
            await NofificarProgreso(3, "Se hizo tarea 3");
            await NofificarProgreso(4, "Se hizo tarea 4");
            await NofificarProgreso(5, "Se hizo tarea 5");
        }
    }
}

