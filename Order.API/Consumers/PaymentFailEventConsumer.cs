using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentFailEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly AppDbContext _Context;
        private readonly ILogger<PaymentFailEventConsumer> _Logger;

        public PaymentFailEventConsumer(AppDbContext context, ILogger<PaymentFailEventConsumer> logger)
        {
            _Context = context;
            _Logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            var order = await _Context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
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
