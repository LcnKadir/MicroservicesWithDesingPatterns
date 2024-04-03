using Automatonymous;
using MassTransit;
using Shared;
using Shared.Events;
using Shared.Interfaces;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public Event<IStockReserveEvent> StockReserveEvent { get; set; }
        public Event<IStockNotReserveEvent> StockNotReserveEvent { get; set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public Event<IPaymentFailedEvent> PaymentFailedEvent { get; set; }

        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State StockNotReserved { get; private set; }
        public State PaymentCompleted { get; private set; }
        public State PaymentFailed { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            //When the event is fired, it will change the state of the related line //Event fırlatıldığında ilgili satırın state'ini değiştirecek
            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReserveEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => StockNotReserveEvent, x => x.CorrelateById(y => y.Message.CorrelationId));


            Initially(When(OrderCreatedRequestEvent).Then(context =>
            {
                context.Instance.BuyerId = context.Data.BuyerId;

                context.Instance.OrderId = context.Data.OrderId;

                context.Instance.CreatedDate = DateTime.Now;
                context.Instance.CardName = context.Data.Payment.CardName;
                context.Instance.CardNumber = context.Data.Payment.CardNumber;
                context.Instance.CVV = context.Data.Payment.CVV;
                context.Instance.Expiration = context.Data.Payment.Expiration;
                context.Instance.TotalPrice = context.Data.Payment.TotalPrice;
            }).Then(context => { Console.WriteLine($"OrderCreatedRequestEvent before : {context.Instance}"); }).TransitionTo(OrderCreated)
            .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId) { OrderItems = context.Data.OrderItems })
            .TransitionTo(OrderCreated)
            .Then(context => { Console.WriteLine($"OrderCreatedRequestEvent after : {context.Instance}"); }));

            During(OrderCreated,
               When(StockReserveEvent)
              .TransitionTo(StockReserved)
               .Send(new Uri($"queue:{RabbitMQSettingsConst.PaymentStockReserveRequestQueueName}"), context => new
             StockReserveRequestPayment(context.Instance.CorrelationId)
               {
                   OrderItems = context.Data.OrderItems,
                   Payment = new PaymentMessage()
                   {
                       CardName = context.Instance.CardName,
                       CardNumber = context.Instance.CardNumber,
                       CVV = context.Instance.CVV,
                       Expiration = context.Instance.Expiration,
                       TotalPrice = context.Instance.TotalPrice
                   },
                   BuyerId = context.Instance.BuyerId
               }).Then(context => { Console.WriteLine($"StockReserveEvent after : {context.Instance}"); }),
               When(StockNotReserveEvent).TransitionTo(StockNotReserved)
               .Publish(cont => new OrderRequestFailedEvent() { OrderId = cont.Instance.OrderId, Reason = cont.Data.Reason })
               .Then(cont => { Console.WriteLine($"StockReserveEvent after : {cont.Instance}"); }));

            During(StockReserved,
                When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .Publish(context => new OrderRequestCompletedEventCunsomer() { OrderId = context.Instance.OrderId })
                .Then(context => { Console.WriteLine($"PaymentCompletedEvent after : {context.Instance}"); }).Finalize(),
                When(PaymentFailedEvent)
                .Publish(cont => new OrderRequestFailedEvent() { OrderId = cont.Instance.OrderId, Reason = cont.Data.Reason })
                .Send(new Uri($"queue:{RabbitMQSettingsConst.StockRollBackMessageQueueName}"), context => new StockRollBackMessage() { OrderItems = context.Data.OrderItems}).TransitionTo(PaymentFailed)
                .Then(context => { Console.WriteLine($"PaymentFailEvent after : {context.Instance}"); })
                );

            SetCompletedWhenFinalized(); // Delete completed transactions from the database // Tamamlanmış olan işlemleri veri tabanından silecek
        }
    }
}
