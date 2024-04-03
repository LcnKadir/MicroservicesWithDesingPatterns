using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StockReserveRequestPayment : IStockReserveRequestPayment
    {
        public StockReserveRequestPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public PaymentMessage Payment { get ; set; }
        public List<OrderItemMessage> OrderItems { get; set; }

        public Guid CorrelationId {  get; }
        public string BuyerId { get; set; }
    }
}
