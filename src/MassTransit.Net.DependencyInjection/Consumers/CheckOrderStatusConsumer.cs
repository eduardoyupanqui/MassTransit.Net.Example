using MassTransit.Net.DependencyInjection.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MassTransit.Net.DependencyInjection.Consumers
{
    public class CheckOrderStatusConsumer : IConsumer<CheckOrderStatus>
    {
        //readonly IOrderRepository _orderRepository;

        public CheckOrderStatusConsumer()
        //public CheckOrderStatusConsumer(IOrderRepository orderRepository)
        {
            //_orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<CheckOrderStatus> context)
        {
            //var order = await _orderRepository.Get(context.Message.OrderId);
            var order = new OrderModel();
            if (order == null)
                throw new InvalidOperationException("Order not found");

            await context.RespondAsync<OrderStatusResult>(new
            {
                OrderId = order.Id,
                order.Timestamp,
                order.StatusCode,
                order.StatusText
            });
        }
    }
}
