
using MassTransit.Net.Quartz.Command;
using MassTransit.Net.Quartz.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransit.Net.DependencyInjection.Controller
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
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/schedule_test_queue"));
            await sendEndpoint.Send<ScheduleNotification>(new
            {
                EmailAddress = "eduardo@outlook.com",
                Body = "<h1>Hola desde Quartz</h1>",
                DeliveryTime = DateTime.Now + TimeSpan.FromSeconds(10)
            });

            return Ok();
        }
    }
}
