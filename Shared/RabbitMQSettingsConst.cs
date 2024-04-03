using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMQSettingsConst
    {
        public const string StockRollBackMessageQueueName = "stock-rollback-queue";

        public const string OrderSaga = "order-saga-queue";
        public const string PaymentStockReserveRequestQueueName = "payment-stock-reserved-request-queue";

        public const string StockReserveEventQueueName = "stock-reserved-queue";
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";


        public const string OrderRequestCompletedEventQueueName = "order-request-completed-queue";
        public const string OrderRequestFailedEventQueueName = "order-request-failed-queue";
        public const string OrderStockNotReservedEvntQueueName = "order-stock-not-reserved-queue";
    }
}
