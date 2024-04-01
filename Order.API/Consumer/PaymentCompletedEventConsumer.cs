using MassTransit;
using Order.API.Models;
using Shared;

namespace Order.API.Consumer
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly AppDbContext _Context;
        private readonly ILogger<PaymentCompletedEventConsumer> _Logger;

        public PaymentCompletedEventConsumer(AppDbContext context, ILogger<PaymentCompletedEventConsumer> logger)
        {
            _Context = context;
            _Logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _Context.Orders.FindAsync(context.Message.OrderId);
            if(order != null)
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
