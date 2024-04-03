using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IStockReserveRequestPayment : CorrelatedBy<Guid>
    {
        public PaymentMessage  Payment { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }

        public string BuyerId { get; set; }
    }
}
