using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Messages;
using Stock.API.Models;

namespace Stock.API.Cunsomers
{
    public class StockRollBackMessageConsumer : IConsumer<IStockRollBackMessage>
    {
        private readonly AppDbContext _context;
        private ILogger<StockRollBackMessageConsumer> _logger;

        public StockRollBackMessageConsumer(AppDbContext context, ILogger<StockRollBackMessageConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<IStockRollBackMessage> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Stock was released");
        }
    }
}
