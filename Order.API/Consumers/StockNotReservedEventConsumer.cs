using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReserveEvent>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentFailEventConsumer> _Logger;

        public StockNotReservedEventConsumer(AppDbContext context, ILogger<PaymentFailEventConsumer> logger)
        {
            _context = context;
            _Logger = logger;
        }
        public async Task Consume(ConsumeContext<StockNotReserveEvent> context)
        {
            var order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
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
