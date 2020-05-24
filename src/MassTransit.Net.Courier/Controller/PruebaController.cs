
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace MassTransit.Net.Courier.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PruebaController : ControllerBase
    {
        private readonly IBusControl _bus;
        public PruebaController(IBusControl bus) 
        {
            _bus = bus;
        }
        /// <summary>
        /// https://localhost:44398/Prueba?request=holaCourier
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> Get(string request)
        {
            if (string.IsNullOrEmpty(request))
            {
                return BadRequest();
            }

            var builder = new RoutingSlipBuilder(NewId.NextGuid());
            //1
            //builder.AddActivity("DownloadImage", new Uri("rabbitmq://localhost/execute_downloadimage"), new
            //{
            //    ImageUri = new Uri("http://images.google.com/someImage.jpg")
            //});
            //2
            //builder.AddActivity("DownloadImage", new Uri("rabbitmq://localhost/execute_downloadimage"));
            //builder.AddVariable("ImageUri", "http://images.google.com/someImage.jpg");
            //3: si el ProcessImage no depende del fin de DownloadImage
            builder.AddActivity("DownloadImage", new Uri("rabbitmq://localhost/execute_downloadimage"));
            builder.AddActivity("ProcessImage", new Uri("rabbitmq://localhost/execute_processimage"));
            builder.AddVariable("ImageUri", "http://images.google.com/someImage.jpg");

            builder.AddActivity("FilterImage", new Uri("rabbitmq://localhost/execute_filterimage"));
            builder.AddVariable("WorkPath", "\\dfs\\work");
            //Events
            //1
            //builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.All);
            //2 modo flags
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"),
                RoutingSlipEvents.Completed | RoutingSlipEvents.Faulted);
            //builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"),
            //    RoutingSlipEvents.Completed, RoutingSlipEventContents.None);
            //3
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.Completed);
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.Faulted);
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.CompensationFailed);
            
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.ActivityCompleted);
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.ActivityFaulted);
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.ActivityCompensated);
            builder.AddSubscription(new Uri("rabbitmq://localhost/log-events"), RoutingSlipEvents.ActivityCompensationFailed);
            // 4 custom public interface OrderProcessingCompleted
            //builder.AddSubscription(new Uri("rabbitmq://localhost/order-events"),
            //    RoutingSlipEvents.Completed,
            //    x => x.Send<OrderProcessingCompleted>(new
            //    {
            //        OrderId = "BFG-9000",
            //        OrderApproval = "ComeGetSome"
            //    }));
            var routingSlip = builder.Build();

            await _bus.Execute(routingSlip);
            return Ok(new { routingSlip.TrackingNumber });
        }
    }
}
