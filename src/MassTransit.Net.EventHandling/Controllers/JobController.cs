using MassTransit.Net.EventHandling.Application.Commands.Job;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.EventHandling.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : BaseController
    {
        private readonly ILogger<JobController> _logger;
        private readonly IMediator _mediator;

        public JobController(ILogger<JobController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> Get(string request)
        {

            var response = await _mediator.Send(new CrearJobCommand {
                IdAplicacion = Guid.NewGuid(),
                CodigoTipoJob = "JobPrueba",
                HostName = "EDUARDO-PC",
                Comentario = "Nothing",
                InputJob = "",
                RegistroAsociado = ""
            });

            return Ok(response);
        }
    }
}
