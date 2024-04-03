using MassTransit;
using Order.API.Models;
using RabbitMQ.Client;
using Shared.Interfaces;

namespace Order.API.Consumers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<OrderRequestFailedEventConsumer> _Logger;

        public OrderRequestFailedEventConsumer(AppDbContext context, ILogger<OrderRequestFailedEventConsumer> logger)
        {
            _context = context;
            _Logger = logger;
        }
        public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Reason;
                await _context.SaveChangesAsync();

                _Logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed: {order.Status}");
            }
            else
            {
                _Logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
