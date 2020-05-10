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
    public class OrderController : ControllerBase
    {
        private readonly IRequestClient<SubmitOrder> _requestClient;

        public OrderController(IRequestClient<SubmitOrder> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost()]
        [Route("Submit")]
        public async Task<IActionResult> Submit(OrderModel model, CancellationToken cancellationToken)
        {
            try
            {
                var request = _requestClient.Create(new { OrderId = Guid.NewGuid() , SomeValue  = model.SomeValue }, cancellationToken);

                var response = await request.GetResponse<OrderAccepted>();

                return Content($"Order Accepted: {response.Message.SomeValue}");
            }
            catch (RequestTimeoutException exception)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
