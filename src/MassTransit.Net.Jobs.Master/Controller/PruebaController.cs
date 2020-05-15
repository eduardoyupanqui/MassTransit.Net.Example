
using MassTransit.Net.Jobs.Client;
using MassTransit.Net.Jobs.Client.Commands;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransit.Net.Jobs.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PruebaController : ControllerBase
    {
        private readonly IBus _bus;
        public PruebaController(IBus bus) 
        {
            _bus = bus;
        }
        /// <summary>
        /// https://localhost:44351/Prueba?queue=FinalizarActividad
        /// https://localhost:44351/Prueba?queue=ReplicarVersionSolicitud
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> Get(string job)
        {
            if (string.IsNullOrEmpty(job))
            {
                return BadRequest();
            }
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri($"queue:{job.ToUnderscoreCase()}"));
            await sendEndpoint.Send<JobCommand>(new
            {
                JobId = Guid.NewGuid(),
                JobInput = "{ name: \"Eduardo\"}",
                CodigoJob = "FINALIZAR_ACTIVIDAD"
            });

            return Ok();
        }
    }
}
