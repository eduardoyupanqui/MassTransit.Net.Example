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

        public override async Task<JobResult> EjecutarJob(JobCommand command)
        {
            await NofificarProgreso(1, "Se hizo tarea 1");
            await NofificarProgreso(2, "Se hizo tarea 2");
            await NofificarProgreso(3, "Se hizo tarea 3");
            await NofificarProgreso(4, "Se hizo tarea 4");
            await NofificarProgreso(5, "Se hizo tarea 5");
            return new JobResult() { OutputJob = "Se termino pé" };
        }
    }
}
