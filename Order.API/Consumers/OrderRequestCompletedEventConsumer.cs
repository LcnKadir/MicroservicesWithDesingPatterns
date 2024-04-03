using MassTransit;
using Order.API.Models;
using Shared.Interfaces;

namespace Order.API.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly AppDbContext _Context;
        private readonly ILogger<OrderRequestCompletedEventConsumer> _Logger;

        public OrderRequestCompletedEventConsumer(AppDbContext context, ILogger<OrderRequestCompletedEventConsumer> logger)
        {
            _Context = context;
            _Logger = logger;
        }
        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var order = await _Context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Complete;
                await _Context.SaveChangesAsync();

                _Logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed: {order.Status}");
            }
            else
            {
                _Logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
