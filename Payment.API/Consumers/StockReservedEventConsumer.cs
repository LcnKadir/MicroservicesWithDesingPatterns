using MassTransit;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReserveEvent>
    {
        private readonly ILogger<StockReservedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(ILogger<StockReservedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReserveEvent> context)
        {
            var balance = 3000m;
            if(balance> context.Message.Payment.TotalPrice)
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} Paranız kartınızdan çekildi id={context.Message.BuyerId}");
           
            await _publishEndpoint.Publish(new PaymentSuccessedEvent { BuyerId = context.Message.BuyerId, OrderId= context.Message.OrderId });
            }
            else
            {
                _logger.LogInformation($"{context.Message.Payment.TotalPrice} Satın alma işlemi gerçekleşemedi id={context.Message.BuyerId}");

                await _publishEndpoint.Publish(new PaymentFailedEvent { BuyerId = context.Message.BuyerId, OrderId = context.Message.OrderId, Message="yeterli bakiye yok" });
            }
        }
    }
}
